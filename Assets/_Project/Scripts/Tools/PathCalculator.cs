using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathCalculator
{ 
    // Route data for path calculations to pass between functions
    private struct RouteContext
    {
        public List<GolfBall> allBalls;
        public float[] timeToStart;
        public float[,] timeBetween;
        public float pickUpDuration;
        public float dropOffDuration;
        public int maxCarry;
        public float totalHealth;
    }
 
    public static (List<Vector3>, List<GolfBall>) CalculatedPath(
        Transform startPoint,
        List<GolfBall> allBalls,
        NpcSettings npcSettings)
    {
        int n = allBalls.Count;
        Vector3 startPos = startPoint.position;
 
        if (n == 0)
            return (new List<Vector3>(), new List<GolfBall>());
        
        RouteContext routeContext = new RouteContext
        {
            allBalls = allBalls,
            pickUpDuration = npcSettings.pickUpDuration,
            dropOffDuration = npcSettings.dropOffDuration,
            maxCarry = npcSettings.maxBallsToCarry,
            totalHealth = npcSettings.healthDuration,
            timeToStart = new float[n],
            timeBetween = new float[n, n]
        };
 
        float speed = npcSettings.speed;
        if (speed <= 0f) speed = 0.001f;
        float acceleration = npcSettings.acceleration;
        
        for (int i = 0; i < n; i++)
        {
            float dist = Vector3.Distance(allBalls[i].transform.position, startPos);
            routeContext.timeToStart[i] = CalculateTravelTime(dist, speed, acceleration);
            
            for (int j = i + 1; j < n; j++)
            {
                float distBetween = Vector3.Distance(allBalls[i].transform.position, allBalls[j].transform.position);
                float t = CalculateTravelTime(distBetween, speed, acceleration);
                routeContext.timeBetween[i, j] = t;
                routeContext.timeBetween[j, i] = t;
            }
        }
 
        // Greedy construction
        List<int> route = BuildGreedyRoute(ref routeContext);
 
        // Local search improvement
        route = LocalSearch(route, ref routeContext);
 
        // Convert to output
        return ConvertToOutput(route, startPos, routeContext.allBalls);
    }
 
    // Greedy Construction
    private static List<int> BuildGreedyRoute(ref RouteContext routeContext)
    {
        List<int> route = new List<int>();
        HashSet<int> collected = new HashSet<int>();
        float healthLeft = routeContext.totalHealth;
        int carrying = 0;
        int currentPos = -1;
        
        while (true)
        {
            int bestBall = -1;
            float bestScore = -1f;
 
            for (int i = 0; i < routeContext.allBalls.Count; i++)
            {
                if (collected.Contains(i)) continue;
                if (routeContext.allBalls[i].GetBallPoints() <= 0f) continue;
 
                float travelTime = GetTravelTime(currentPos, i, ref routeContext);
                float costToGet = travelTime + routeContext.pickUpDuration;
                float returnCost = routeContext.timeToStart[i] + routeContext.dropOffDuration;
 
                if (healthLeft - costToGet - returnCost < 0f)
                    continue;
                
                float score = routeContext.allBalls[i].GetBallPoints() / Mathf.Max(costToGet, 0.001f);
 
                if (score > bestScore)
                {
                    bestScore = score;
                    bestBall = i;
                }
            }
 
            if (bestBall == -1)
                break;
 
            healthLeft -= GetTravelTime(currentPos, bestBall, ref routeContext) + routeContext.pickUpDuration;
            route.Add(bestBall);
            collected.Add(bestBall);
            carrying++;
            currentPos = bestBall;
 
            if (carrying >= routeContext.maxCarry)
            {
                healthLeft -= routeContext.timeToStart[currentPos] + routeContext.dropOffDuration;
                route.Add(-1);
                carrying = 0;
                currentPos = -1;
            }
        }
        
        // Deliver remaining balls
        if (carrying > 0 && currentPos != -1)
        {
            float returnCost = routeContext.timeToStart[currentPos] + routeContext.dropOffDuration;
 
            if (healthLeft >= returnCost)
            {
                route.Add(-1);
            }
            else
            {
                while (route.Count > 0 && route[^1] != -1)
                    route.RemoveAt(route.Count - 1);
            }
        }
 
        return route;
    }
    
    // Local Search
    private static List<int> LocalSearch(List<int> route, ref RouteContext ctx, int maxIterations = 30)
    {
        bool improved = true;
        int iteration = 0;
 
        while (improved && iteration < maxIterations)
        {
            improved = false;
            if (!TrySwap(ref route, ref ctx))
                improved = TryInsert(ref route, ref ctx);
            else
                improved = true;
            iteration++;
        }
 
        // Final pass: optimize order for shortest time (same points, less health used)
        TryReorder(ref route, ref ctx);
 
        return route;
    }
 
    private static bool TrySwap(ref List<int> route, ref RouteContext ctx)
    {
        float bestPoints = CalculatePoints(route, ctx.allBalls);
 
        for (int i = 0; i < route.Count; i++)
        {
            if (route[i] == -1) continue;
 
            for (int k = 0; k < ctx.allBalls.Count; k++)
            {
                if (route.Contains(k)) continue;
                if (ctx.allBalls[k].GetBallPoints() <= ctx.allBalls[route[i]].GetBallPoints())
                    continue;
 
                int old = route[i];
                route[i] = k;
 
                if (IsValid(route, ref ctx))
                {
                    float newPoints = CalculatePoints(route, ctx.allBalls);
                    if (newPoints > bestPoints)
                        return true;
                }
 
                route[i] = old;
            }
        }
 
        return false;
    }
 
    private static bool TryInsert(ref List<int> route, ref RouteContext ctx)
    {
        float bestPoints = CalculatePoints(route, ctx.allBalls);
 
        for (int k = 0; k < ctx.allBalls.Count; k++)
        {
            if (route.Contains(k)) continue;
            if (ctx.allBalls[k].GetBallPoints() <= 0f) continue;
 
            // Try inserting within existing trips
            for (int pos = 0; pos < route.Count; pos++)
            {
                if (GetCarryCountAt(route, pos) >= ctx.maxCarry) continue;
 
                route.Insert(pos, k);
 
                if (IsValid(route, ref ctx))
                {
                    float newPoints = CalculatePoints(route, ctx.allBalls);
                    if (newPoints > bestPoints)
                        return true;
                }
 
                route.RemoveAt(pos);
            }
 
            // Try creating a new trip at the end: [existing..., ball, -1]
            route.Add(k);
            route.Add(-1);
 
            if (IsValid(route, ref ctx))
            {
                float newPoints = CalculatePoints(route, ctx.allBalls);
                if (newPoints > bestPoints)
                    return true;
            }
 
            route.RemoveAt(route.Count - 1);
            route.RemoveAt(route.Count - 1);
        }
 
        return false;
    }
 
    // swap pairs of balls within each trip to minimize travel time.
    private static void TryReorder(ref List<int> route, ref RouteContext ctx)
    {
        bool improved = true;
 
        while (improved)
        {
            improved = false;
 
            for (int i = 0; i < route.Count; i++)
            {
                if (route[i] == -1) continue;
 
                for (int k = i + 1; k < route.Count; k++)
                {
                    if (route[k] == -1) continue;
 
                    float oldTime = CalculateRouteTime(route, ref ctx);
 
                    // Swap
                    int temp = route[i];
                    route[i] = route[k];
                    route[k] = temp;
 
                    if (IsValid(route, ref ctx))
                    {
                        float newTime = CalculateRouteTime(route, ref ctx);
                        if (newTime < oldTime)
                        {
                            improved = true;
                            continue; // Keep the swap
                        }
                    }
 
                    // Revert swap
                    temp = route[i];
                    route[i] = route[k];
                    route[k] = temp;
                }
            }
        }
    }
    
    // Calculates travel time accounting for acceleration from 0 to max speed.
    private static float CalculateTravelTime(float distance, float maxSpeed, float acceleration)
    {
        if (distance <= 0f) return 0f;
        if (acceleration <= 0f) return distance / maxSpeed;
 
        // Time to reach max speed
        float timeToMaxSpeed = maxSpeed / acceleration;
        
        // Distance covered during acceleration
        float distDuringAccel = (maxSpeed * maxSpeed) / (2f * acceleration);
 
        if (distance <= distDuringAccel)
        {
            // Never reaches max speed
            return Mathf.Sqrt(2f * distance / acceleration);
        }
        else
        {
            // Accelerates to max speed, then completed rest on max speed
            float remainingDist = distance - distDuringAccel;
            return timeToMaxSpeed + (remainingDist / maxSpeed);
        }
    }
 
    private static float GetTravelTime(int fromPos, int toBallIndex, ref RouteContext ctx)
    {
        return (fromPos == -1) ? ctx.timeToStart[toBallIndex] : ctx.timeBetween[fromPos, toBallIndex];
    }
 
    /// Calculates total time cost of a route (travel + pickups + dropoffs).
    private static float CalculateRouteTime(List<int> route, ref RouteContext ctx)
    {
        float time = 0f;
        int prev = -1;
 
        for (int i = 0; i < route.Count; i++)
        {
            int cur = route[i];
 
            if (cur == -1)
            {
                time += ((prev == -1) ? 0f : ctx.timeToStart[prev]) + ctx.dropOffDuration;
            }
            else
            {
                time += GetTravelTime(prev, cur, ref ctx) + ctx.pickUpDuration;
            }
 
            prev = (cur == -1) ? -1 : cur;
        }
 
        return time;
    }
 
    private static bool IsValid(List<int> route, ref RouteContext ctx)
    {
        if (route.Count == 0) return true;
        
        if (route[^1] != -1) return false;
 
        float health = ctx.totalHealth;
        int prev = -1;
        int carrying = 0;
 
        for (int i = 0; i < route.Count; i++)
        {
            int current = route[i];
 
            if (current == -1)
            {
                if (carrying == 0) return false;
                health -= ((prev == -1) ? 0f : ctx.timeToStart[prev]) + ctx.dropOffDuration;
                carrying = 0;
            }
            else
            {
                if (carrying >= ctx.maxCarry) return false;
                health -= GetTravelTime(prev, current, ref ctx);
                health -= ctx.pickUpDuration;
                carrying++;
            }
 
            if (health < 0f) return false;
            prev = (current == -1) ? -1 : current;
        }
 
        return true;
    }
 
    private static float CalculatePoints(List<int> route, List<GolfBall> allBalls)
    {
        float total = 0f;

        for (int i = 0; i < route.Count; i++)
        {
            if (route[i] != -1)
            {
                total += allBalls[route[i]].GetBallPoints();
            }
        }
 
        return total;
    }
 
    private static int GetCarryCountAt(List<int> route, int position)
    {
        int count = 0;
        for (int i = position - 1; i >= 0; i--)
        {
            if (route[i] == -1) break;
            count++;
        }
        return count;
    }
 
    private static (List<Vector3>, List<GolfBall>) ConvertToOutput(
        List<int> route, Vector3 startPos, List<GolfBall> allBalls)
    {
        List<Vector3> path = new List<Vector3>();
        List<GolfBall> targetBalls = new List<GolfBall>();
 
        foreach (int idx in route)
        {
            if (idx == -1)
            {
                path.Add(startPos);
                targetBalls.Add(null);
            }
            else
            {
                path.Add(allBalls[idx].transform.position);
                targetBalls.Add(allBalls[idx]);
            }
        }
 
        return (path, targetBalls);
    }
}

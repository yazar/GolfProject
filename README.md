# Golf Course NPC Ball Collector

## 1. Game Mechanics

The game features an NPC character that must collect golf balls scattered across the environment randomly and deliver them to a starting point to earn points. We can observe the pattern of the NPC and adjust some settings like the amount of time it has to collect balls, the number of balls it can carry on itself before delivering them etc.

### Core Constraints

- **Health as Time:** The NPC has a health value that starts decreasing the moment it begins moving and never stops. Health acts as the total time budget.
- **Carrying Capacity:** The NPC can only carry a limited number of balls at once. When full, it must return to start to deliver before collecting more.
- **Delivery Requirement:** Points are only awarded when balls are physically delivered to the starting point. Undelivered balls yield zero points.
- **Pickup & Dropoff Duration:** Both collecting a ball and delivering balls consume time (health) while their animations play.
- **Acceleration:** The NPC uses a NavMeshAgent with acceleration. It does not instantly reach max speed, which affects real travel time calculations.
- More constraints can be found under: `Assets/_Project/Settings`

**Objective:** Maximize total points collected and delivered before health reaches zero.

## 2. Algorithm Design

The solution uses a three-phase approach: Greedy Construction, Local Search, and Route Reordering.

### Phase 1: Greedy Construction

Builds an initial route by iteratively selecting the best available ball using a points per time efficiency metric:

```
score = ball.points / timeToReachBall
```

This balances distance and value. A nearby low value ball may score higher than a distant high value ball, or vice versa.

**At each step:**

1. For every uncollected ball, calculate the efficiency score.
2. Verify there is enough remaining health to: travel to the ball + pick it up + return to start + deliver.
3. Select the ball with the highest efficiency score.
4. If carrying capacity is full after pickup, automatically return to start and begin a new trip.

The health budget check ensures survival: before selecting any ball, the algorithm reserves enough health for the return trip. The NPC will never strand itself.

### Phase 2: Local Search Improvement

Refines the greedy solution with two operators, iterated until no improvement is found (max 30 iterations):

- **TrySwap:** Attempts to replace a ball in the route with a higher value ball not currently in the route. Accepts the first swap that increases total points and passes validation.
- **TryInsert:** Attempts to add a new ball to the route at any valid position within existing trips. Also attempts creating entirely new trips. Accepts the first insertion that increases total points.

### Phase 3: Route Reordering

After the route's ball selection is finalized, this phase optimizes visit order to minimize total travel time. The same set of balls can be collected via different orderings with significantly different path lengths.

**Method:** For every pair of balls in the route, swap their positions. If the swap reduces total route time while remaining valid, keep it. Repeat until no improvement is found (max 20 iterations).

This phase does not change which balls are collected, only the order, saving health for the same point total.

## 3. Travel Time Calculation

Standard distance/speed calculation underestimates real NavMeshAgent travel time due to acceleration from rest. The algorithm uses kinematic equations:

**Short distance (agent never reaches max speed):**

```
time = sqrt(2 * distance / acceleration)
```

**Long distance (agent reaches max speed, then cruises):**

```
accelTime = maxSpeed / acceleration
accelDistance = maxSpeed² / (2 * acceleration)
cruiseDistance = totalDistance - accelDistance
time = accelTime + cruiseDistance / maxSpeed
```

All travel times are pre-calculated into lookup tables for O(1) access during route evaluation.

## 4. Route Validation

Every candidate route modification is validated before acceptance. The validation checks:

- Route must end with a return to the start marker.
- No empty trips (consecutive returns without ball pickups).
- Carrying capacity is never exceeded within any trip.
- Health never drops below zero at any point in the route.

## 5. Data Architecture

A `RouteContext` struct holds all per calculation data: pre computed travel time arrays, NPC settings (health, speed, capacity, durations), and ball references.

Each `CalculatedPath` call creates its own context with no shared static state. This is added so if we want to add multiple NPCs in the future they can calculate paths simultaneously without data interference, making the system thread safe.

**Build Link :** https://drive.google.com/drive/folders/1cqYWNMwfabgl4AKPXckIccoRRo9TTr9w?usp=sharing
**Short Gameplay Video :** https://drive.google.com/file/d/11YJ5zhzR_IivfaMjEDsnZ_1cTL5XC6qG/view?usp=sharing

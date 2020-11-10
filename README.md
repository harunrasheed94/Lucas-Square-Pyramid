## COP5615 DISTRIBUTED OPERATING SYSTEMS

## PROJECT 1 
Finding perfect squares that are sums of consecutive squares

## Group Members
Mohammed Haroon Rasheed Kalilur Rahman - 6751 2967 \
Ariz Ahmad - 7111 2167

## Problem Description
An interesting problem in arithmetic with deep implications to elliptic curve theory is the problem of finding perfect squares that are sums of consecutive squares. A classic example is the Pythagorean identity.

Sums of squares of consecutive integers form the
square of another integer.
The goal of this first project is to use F# and the actor model to build a good solution to this problem that runs well on multi-core machines.



## Implementation Details

lucasSqrChecker is our method to check if the sum of squares of k consecutive values starting from s are perfect square or not.

We have a Boss Actor which defines worker actors that are given a range of problems to solve andthe boss keeps track of all the problems and performs the job assignment. Child Actors call the function lucasSqrChecker to check sum of squares. These values are returned by the respective child actors to the Boss Actor only if they are perfect squares. The values are then printed by the Boss Actor.



## Execution
we use the following command in our prompt to run the file:

dotnet fsi --langversion:preview lucas.fsx 1000000 4

The Real time, CPU time, and the ratio of CPU time vs Real time are computed and printed after the program has finished execution.

Finally, we press return key to terminate the program.



## Size of Work Unit
We ran our project with 10, 100, 500 and 1000 work units. We observed that dividing the work unit N/100, gave the best result in our system. Boss Actor divides the numbers into as many groups as the number of worker actors. For example, in our program, for N = 1000000, the Boss Actor divides the set of numbers into 100 groups of 10,000 each and assigns to each of the individual workers. The program accepts the values of N and K from the user, and makes the boss assign these to the actors.

 


## Result of running the program 
We do not obtain any solution for N = 1000000 and K = 4, because the solution for Lucas' Square Pyramid does not exist for these values.



## Running time 
The following is the observation for the above query:
Real Time for Calculation : 1740 ms
CPU Time : 5234 ms
Number of Cores : 4
Ratio of CPU Time to Real Time = 3.008046 


## Largest problem solved
The largest problem our program solved was for the values 100000000 24
Following is the observation for the above query:
Real Time for Calculation : 189573 ms
CPU Time : 474312 ms
Number of Cores : 4
Ratio of CPU Time to Real Time = 2.502002
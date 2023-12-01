# AoCCSharpSkeleton
Advent of Code C# Skeleton

This is a simple skeleton project for completing Advent of Code in C#.

It includes a single solution to use as an example. Copying the FuelSum class is an easy way to get started.

To do your own solution, grab the problem text, example data, and actual data from your Advent of Code problem and put them in 3 files in the Resources folder.
You can put them anywhere, however I prefer /Resources/Year[YEAR]/Day[DAY]/.

Then, create a new class (I put it in a matching folder structure as the resources) which inherites from SolutionMain.
SolutionMain will get you to implement a Solve(List String) method. This is where you can put your solution, returning your String answer.
You will also need to create a constructor which calls SolutionMains constructor, setting the resource path. E.g. "Year2019/Day1/"
SolutionMain gives you access to several helper methods, including:
 - ConvertToIntegerList
 - ConvertToLongList
 - PrintInfo (controlled by a verbose flag)
 
To run your solution, inside the Program Main method, create a new instance of your class and call one of the following methods on it:
 - PrintSolution (runs your solution against the example and actual data)
 - PrintExample (only runs against the example data, not the actual data)
 - TimeSolution (Pass it a number of runs and it will time how long your solution takes, for both example and actual data)

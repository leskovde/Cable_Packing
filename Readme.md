# User Manual

**Browse**: select a file from the file explorer, the file should contain the radii of wires (in millimeters, with dot as decimal separator), one per line. 

**Algorithm selection**: choose which algorithm should be run 

- Greedy – Closer to the optimum packing while being slower than other algorithms 
- Single Pass – Faster at the cost of giving sub-optimal results 
- Approximation – (work in progress) Closest to the optimum packing at reasonable speed 

**Run**: Executes the selected algorithm on the chosen input file. The output is shown on the canvas below. The radii of the circles are printed at their center. The diameter of  the wire bundle is written in the control panel, below the Browse button. 

 

# Developer Documentation

The circle packing problem is considered NP-hard. One of the most accurate approximations of the problem involves solving a mixed integer program, which is attempted in this implementation as well. However, due to the constraint of the input to integers (or rational numbers), one can use naïve approaches to get a usable result. Therefore, this program tries to implement two polynomial algorithms with decent speed and accuracy.  

## The computational pipeline 

Firstly, in the `StartButton_Click` event, a file is parsed via the `InputProcessor`, found in the `Parser.cs` file. Each line is read and either returned as a valid token or thrown away in case of the first character being a hashmark. Each valid token is then is then parsed into a `decimal` number to capture as much detail as possible. The number with the most decimal places sets the maximum precision, which the program uses to convert decimal numbers to integers (using a multiplication with a single factor). 

The tokens are then stored in a list and interpreted further in the pipeline as radii. Based on the user’s choice, an algorithm flag is set in form of an `enum`. The flag serves as the key in a factory dictionary. An algorithm wrapper is made from the selected factory, which is then called using a **strategy pattern**. The result of the algorithm is a structure holding the objects with radii and centers of all inner circles and the outer circle. These objects are structures wrapping two long integers, that is the center of the circle, a single integer being the radius and a random color. The result is then passed via a delegate to the `Paint` function, which visualizes the output onto the canvas. 

## The input 

The input of the program is a single file, each line of which holds either a numerical string with a dot as a decimal separator, or a comment starting with a hashmark. Comments are ignored. The file uses the ASCII coding. 

## The output 

The program writes the diameter of the wire bundle to a label in the UI. It also visualizes the placement of each circle and its radius using the `PaintEvent` API. 

## Algorithms 

The single pass and greedy algorithm share some similarities. They share an imaginary grid that serves as a discrete two-dimensional space. Points can only be placed onto the grid. The grid has the size of the sum of all input radii for both of its axes. Roughness tells the algorithm how accurately it should run. Roughness translates equally to the size of a single step in the grid. The higher the roughness, the faster the grid can be traversed. Roughness is by default set to `gridSize/1000.`But it can also be changed inside the code manually. Different roughness values contribute to different resulting packings. Another common quality is the fitting of the wire bundle. After all  inner circles have been placed, a grid scan ensures that the center of  the outer circle is in its best position and has the lowest possible radius.
### The greedy algorithm 

Giving the best results, the greedy algorithm is currently the most accurate circle packing algorithm currently implemented. First, the input is sorted in a descending manner, so that the largest circles are placed first. Then, for each circle that has yet to be placed, we find a position closest to the center of the grid that is not occupied. This  involves scanning the grid for each circle, resulting in an `O(m + c)` time complexity, where `m` is the number of circles and `c` is a constant number of steps needed to traverse the grid. 

### The single pass algorithm 

Using a spiral to then place every circle in a single grid scan, the single pass algorithm achieves subpar results. Many heuristics can be made. This implementation tries to distribute the circles onto the spiral in a more balanced way. First, the circles are split into two halves after being sorted.  Second, the circles are then placed onto the spiral. If the current  spiral point does not fit a large circle, a smaller circle is chosen. This results in a runtime of `O(c)`. 

### The mixed integer program 

Although not currently usable, the mixed integer program is written in the code and should be correctly formulated. The idea is to build a system of equations and finds the optimum value with respect to a target function. In this case, the target function would focus on minimizing the radius of the wire bundle, while the equation system gives constraints needed for this task. First of which being that the inner  circles cannot overlap and the second that the outer circle must contain all inner circles completely. The Google.OR.Tools package provides a wrapper for many different LP and MIP solvers, but nearly none of them support variable multiplication. This approach needs further testing and debugging as well as more knowledge of the solver itself. The code can be found in the `IntegerProgrammingPackingAlgorithm` class.

## Possible Improvements

While the MIP should by far be the best solution to this problem, there are many improvements that can be made to the previous two algorithms. The grid does not need to be present at all, distances of inner circles can be made relative to each other. Different starting positions may yield better results and thus might be worth trying. The single pass algorithm could be converted to an iteratively extending one, which would increase the radius of the spiral and try solving a subproblem in each iteration. Different heuristics can be employed in order to reduce the scan time in the greedy algorithm. Testing multiple iterations with different roughness levels in a single run might also give better results.

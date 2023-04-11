```
Author:     Sam Henderson
Partner:    None
Date:       21-Feb-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  SamHenderson-1
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain
Commit #:   5237fec38594ad5129ca74166120dbffbfe018e6
Project:    Onward to a Spreadsheet
Copyright:  CS 3500 and Sam Henderson - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

Speaks for itself

# Assignment Specific Topics
None

# Consulted Peers:
None

# Examples of Good Software Practice:

- Firstly, I have developed a plethora of helper methods throughout the Spreadsheet library that have 
prevented me from overusing more cumbersome pieces of code. The best example I have for this is the 
ReadFile() helper method, which is a massive, complex method of ifs, whiles, and switch/cases meant
to be used in the 3rd constructor and the GetSavedVersion() method. If this helper method was not 
used, the code would be far more needlessly bulky

- While Spreadsheet has plenty of weighty methods, I have done a solid job of condensing them down
into very short, readable pieces of code. Functions like GetSavedVersion() is reduced to a single 
helper method and GetCellValue() is only about 10-11 lines at most. Meanwhile, more complex code, such as
SetContentsOfCell(), are heavily commented so that it may be easily explained. 

- Thanks to the above points, I have not needed to repeat code nearly as often as I would otherwise. 
Most of the methods present are incredibly unique from one another outside of checks for exceptions that
typically appear at the beginning of each one. The only exception to code reuse has been the 
SetCellContents() from Assignment 4, but they are unique in enough ways to justify seemingly repeated code. 
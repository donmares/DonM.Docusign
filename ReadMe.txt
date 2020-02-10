This solution was developed using C# .NET Core 2.2 with Visual Studio Version 4.8.03761

There are three projects:
  Console - consists of the console application to execute the command line interface
  Library - consists of all of the logic to execute this application
  Tests - consists of various tests for development testing purposes

To execute a proximity search, execute the console application with the following parameters:
  <firstKeyword> <secondKeyword> <range> <filename>

Proximity search in this case, is defined as searching all combinations of the range within the search file/string. 
Any time both keywords are found within range of one another, its a match and added to the output. 
The same two words that match cannot be counted more than once. 
So even if the words at positions 2 and 5 are found in multiple ranges, that is counted only as one match since its already been counted in one range. 

The matches are case insensitive so in other words "the" is the same as "The".

The output is the distinct number of matches within the file/string. 

Future Enhancements could include doing pretty much all of the logic using Linq.

This solution is intended solely for the use of a coding exercise for DocuSign.
Please respond with comments to donmares@gmail.com


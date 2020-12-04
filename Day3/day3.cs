using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day3{
class Main {
  public void solve () {
      Console.WriteLine("Day 3:");
      
      var solver = new solver();
      
      string textFile ="Day3/input.txt";
      //string textFile ="Day3/inputExample.txt";

      if (File.Exists(textFile)){
        string[] lines = File.ReadAllLines(textFile);
      foreach (string line in lines){
        Console.WriteLine(line);
      }
        solver.task1(lines);
        //solver.task2(lines);
      }
      else {
        Console.WriteLine("Fant IKKE filen");
      }
  }
}

class solver {
  public void task1(string[] map){
    int height = map.Length;
    int width = map[0].Length;
    
    int row0 = 0;
    int col0 = 0;
    int down = 1;
    int right = 3;
    
    int movesToBottom = (height-1)/down;
    int minCanvasWidth = movesToBottom * right;
    int mapsToAdd = (int)(minCanvasWidth / width);
    Console.WriteLine(mapsToAdd);
    
    // Move in map:
    // Hopp ned og bort til jeg når bunnen
    // registrer hvis tre for hvert Hopp
    // hvis utenfor kartet, fortsett fra andre siden (infinite BC)
    int row = 0;
    int col = 0;
    int move = 0;
    string tree = "#";
    int treeCount = 0;
    while (move < movesToBottom){
      row += down;
      col += right;
      
      // Check if inside map
      if (col >= width){
        col = col - width;
      }
      if (row > height - 1){
        break;
      }
      
      // Sjekk for tre
      //Console.WriteLine(map[row]);
      //Console.WriteLine(row);
      //Console.WriteLine(col);
      //Console.WriteLine(map[row][col]);
      if (map[row][col].ToString().Equals(tree)){
        treeCount += 1;
      }
      
    }
    Console.WriteLine($"Tree count: {treeCount}");
  }
}

}

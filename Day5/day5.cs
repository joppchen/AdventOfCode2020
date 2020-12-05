using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day5{
class Main {
  public void solve () {
      Console.WriteLine("Day 5:");
      
      var solver = new solver();
      
      string textFile ="Day5/input.txt";
      //string textFile ="Day5/inputExample.txt";

      if (File.Exists(textFile)){
        string[] lines = File.ReadAllLines(textFile);
      //foreach (string line in lines){
      //  Console.WriteLine(line);
      //}
      
        solver.task1(lines);
        //solver.task2(lines);
      }
      else {
        Console.WriteLine("Fant IKKE filen");
      }
  }
}

class solver {
  public void task1(string[] BSPs){
    var allRows = (low: 0, high: 128);
    var allCols = (low: 0, high: 7);
    var highestSeatID = 0;
    
    foreach (var bsp in BSPs){
      
      var rowRng = allRows;
      for (int i = 0; i < 7; i++){
        string key = bsp[i].ToString();
        if (key.Equals("F")){
          rowRng = lowerHalf(rowRng);
        }
        if (key.Equals("B")){
          rowRng = upperHalf(rowRng);
        }
      }
      //Console.WriteLine(rowRng);
      
      var colRng = allCols;
      for (int i = 7; i < 10; i++){
        string key = bsp[i].ToString();
        if (key.Equals("L")){
          colRng = lowerHalf(colRng);
        }
        if (key.Equals("R")){
          colRng = upperHalf(colRng);
        }
      }
      //Console.WriteLine(colRng);
      
      var seatID = rowRng.low * 8 + colRng.low;
      //Console.WriteLine(seatID);
      
      if (seatID > highestSeatID) highestSeatID = seatID;
    }
    Console.WriteLine(highestSeatID);
  }
  
  private (int low, int high) lowerHalf((int low, int high) range){
    
    var width = range.high - range.low;
    range.high = range.high - (width + 1) / 2;
    return range;
  }
  
  private (int low, int high) upperHalf((int low, int high) range){
    
    var width = range.high - range.low;
    range.low = range.low + (width + 1) / 2;
    return range;
  }
}

}

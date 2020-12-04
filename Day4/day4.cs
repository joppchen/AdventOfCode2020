using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day4{
class Main {
  public void solve () {
      Console.WriteLine("Day 4:");
      
      var solver = new solver();
      
      //string textFile ="Day4/input.txt";
      string textFile ="Day4/inputExample.txt";
      if (File.Exists(textFile)){
        string text = File.ReadAllText(textFile);
        Console.WriteLine(text.Length);
        solver.task1(text);
        //solver.task2(text);
      }
      else {
        Console.WriteLine("Fant IKKE filen");
      }
  }
}

class solver {
  public void task1(string passports){
    string[] pports;
    char[] separators = {'-', ' ', ':'};
    
    pports = passports.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
    Console.WriteLine($"No. of passports: {pports.Length}");
    
    var i = 0;
    foreach (var pport in pports){
      i += 1;
      Console.WriteLine(i);
      Console.WriteLine(pport);
    }
    
    string[] reqFields = new string[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
    var validPassports = 0;
    
    foreach (var p in pports){
      //Console.WriteLine(p.Contains("cid"));
      
      bool valid = false;
      foreach (string field in reqFields){
        valid = p.Contains(field);
        if (!valid) break;
      }
      Console.WriteLine(valid);
      if (valid) validPassports += 1;
    }
    
    Console.WriteLine($"Valid passports: {validPassports}");
  }
}

}

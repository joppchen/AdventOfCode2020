using System;
using System.Collections.Generic;

namespace day2{
class Main {
  public void solve () {
      Console.WriteLine("Day 2:");
      
      //Test:
      passwords testPwords = new passwords(examplePwords.min, examplePwords.max, examplePwords.letter, examplePwords.password);
      var solver = new solver();
      solver.task1(testPwords);
  }
}

class solver {
  public void task1(passwords pwords){
    var length = pwords.Min.Count;
    int index1;
    int count = 0;
    
    for (int i = 0; i < length; i++){
      // Lag rutine som finner antall bokstaver i passord, kan bruke dette med start og stopp:
      //index1 = pwords.Password[i].IndexOf(pwords.Letter[i]);
      //Console.WriteLine($"index1: {index1}");
      
      count = countTextsInString(pwords.Password[i], pwords.Letter[i], count);
      Console.WriteLine($"Password: {pwords.Password[i]}");
      Console.WriteLine($"Letter: {pwords.Letter[i]}");
      Console.WriteLine($"count: {count}");
      count = 0;
    }
  }
  
  private int countTextsInString(string str, string text, int count, int start = 0){
      //int count = 0;
      //Console.WriteLine(str);
      //Console.WriteLine(text);
      //Console.WriteLine(count);
      int index = str.IndexOf(text, start);
      //Console.WriteLine(index);
      if (index > -1) {
        count += countTextsInString(str, text, count, index+1) + 1;
      }
      return count;
  }
}

class passwords{
  public List<int> Min {get;}
  public List<int> Max {get;}
  public List<string> Letter {get;}
  public List<string> Password {get;}
  
  public passwords(List<int> min, List<int> max, List<string> letter, List<string> password){
    Min = min;
    Max = max;
    Letter = letter;
    Password = password;
  }
}

static class examplePwords{
  public static List<int> min = new List<int>(){1, 1, 2};
  public static List<int> max = new List<int>(){3, 3, 9};
  public static List<string> letter = new List<string>(){"a","b","c"};
  public static List<string> password = new List<string>(){"abcde","cdefg","ccccccccc"};
  
//1-3 a: abcde
//1-3 b: cdefg
//2-9 c: ccccccccc
}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day2{
class Main {
  public void solve () {
      Console.WriteLine("Day 2:");
      
      //Test:
      passwords testPwords = new passwords(examplePwords.min, examplePwords.max, examplePwords.letter, examplePwords.password);
      var solver = new solver();
      solver.task1(testPwords);
      
      string str = "1-3 a: abcde";
      char[] separators = {'-', ' ', ':'};
      string[] words = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
      
      var wordList = new List<string>();
      wordList = words.ToList();
      
      foreach (string word in words){
        Console.WriteLine(word);
      }
      
      string textFile ="Day2/input.txt";
      if (File.Exists(textFile)){
        Console.WriteLine("Fant filen.");
        string[] lines = File.ReadAllLines(textFile);
        //foreach (string line in lines){
          //Console.WriteLine(line);
        //}
      }
      else {
        Console.WriteLine("Fant IKKE filen");
      }
        
  }
}

class solver {
  public void task1(passwords pwords){
    var length = pwords.Min.Count;
    int index1;
    int count = 0;
    int validPasswords = 0;
    
    for (int i = 0; i < length; i++){
      
      count = countTextsInString(pwords.Password[i], pwords.Letter[i]);
      Console.WriteLine($"Password: {pwords.Password[i]}");
      //Console.WriteLine($"Letter: {pwords.Letter[i]}");
      //Console.WriteLine($"count: {count}");
      
      if (count >= pwords.Min[i] && count <= pwords.Max[i]){
        Console.WriteLine("Valid password!");
        validPasswords += 1;
      }
      else Console.WriteLine("Invalid password!");
      
      count = 0;
    }
    
    Console.WriteLine($"Valid passwords: {validPasswords}");
  }
  
  private int countTextsInString(string str, string text, int start = 0){
    
      int index = str.IndexOf(text, start);
      
      if (index < 0) return 0;
      return 1 + countTextsInString(str, text, index+1);
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

static class allPasswords{
  public static List<string> pwords = new List<string>(){
    
  };
  
  
}
}

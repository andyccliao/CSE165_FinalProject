using System;

public static class StringExtension  {


    // Extension Directly Stolen From:
    // https://stackoverflow.com/questions/9367119/replacing-a-char-at-a-given-index-in-string
    public static string ReplaceAt(this string input, int index, char newChar)
    {
        if (input == null) {
            throw new ArgumentNullException("input");
        }
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }
}

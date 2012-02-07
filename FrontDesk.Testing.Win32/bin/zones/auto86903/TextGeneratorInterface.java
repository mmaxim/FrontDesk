import java.io.File;

/** 
 *  An interface for a Text Generator.
 *
 *  You should not modify or handin this file.
 *
 *  You should read the comments very careful for specifics on how you should
 *  implement these functions.
 *
 *  @author Colin Rothwell
 */
public interface TextGeneratorInterface
{
    /**
     *  Takes as input three files: the input file, the data file, and the 
     *  output file.  This method should build a trie corresponding to the 
     *  data file and then use the input file to generate text and store the
     *  results in the output file.  The format of the output file is 
     *  specified in the write up.
     *
     *  Error handling:  If the input file does not exist, return -2.  If the
     *  input file is invalid (does not meet specifications), return -3.  If
     *  the data file does not exist, return -1.  If the output file does not
     *  exist this is not an error, rather you should create it.  If no error
     *  conditions apply, return 0.
     */
    public int generateText(File input, File data, File output);

    /**
     *  Given the data file, this method should build the corresponding trie.
     *  Specifically when this method is finished the data structure should be
     *  entirely built such that calls to generateSentence can do so for any
     *  provided 2-gram without needing to look at the original data file.
     *
     *  Error handling:  If the data file does not exist, return -1.  Otherwise
     *  this method should return 0.  The data file being empty is not an 
     *  error condition.
     */
    public int buildTrie(File f);

    /**
     *  Given a string, this method should attempt to interpret it as
     *  containing a 2-gram.  It should then use the already constructed trie
     *  to generate an aditional n words and return the final string.
     *
     *  Error handling: If the string contains less than 2 words this method 
     *  should return null.  If the trie has not yet been constructed
     *  (generateText or buildTrie have no yet been called), also return null.
     *  If n is not a positive value, return null.  If the string contains more
     *  than two words, this is not an error condition and you should use only
     *  the final two words to generate new text.
     */
    public String generateSentence(String s, int n);

    /**
     *  Returns the trie that you have constructed.  
     *
     *  Error handling: If generateText or buildTrie has not been called this 
     *  method should return null.
     */
    public Trie getTrie();


    /**
     *  Returns the number of words in the original data file
     *
     *  Error handling: If generateText or buildTrie has not been called this 
     *  method should return -1.
     */
    public int numWords();
}
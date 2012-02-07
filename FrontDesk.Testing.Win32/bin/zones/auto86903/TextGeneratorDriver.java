import java.io.File;

/** 
 *  Basic driver class for TextGenerator.
 * 
 *  You may modify this class if you wish, we will not use it for testing.
 *
 *  You should not handin this file.
 *
 *  @author Colin Rothwell
 *  @version 1.0
 *  @see TextGenerator
 *  @see Trie
 */
public class TextGeneratorDriver
{
    public static void main(String[] args)
    {
        File input, data, output;

        if(args.length < 3){
            System.out.print("usage: java TextGeneratorDriver ");
            System.out.print("<input file> <data file> <output file>\n");
            System.exit(0);
        }
        
        input = new File(args[0]);
        data = new File(args[1]);
        output = new File(args[2]);
        
        TextGenerator textGen = new TextGenerator();
        int error = textGen.generateText(input, data, output);
        if(error == -2){
            System.out.println("Failed to open input file");
        }
        else if(error == -3){
            System.out.println("Invalid input file");
        }
        else if(error == -1){
            System.out.println("Failed to open data file");
        }
    }
}

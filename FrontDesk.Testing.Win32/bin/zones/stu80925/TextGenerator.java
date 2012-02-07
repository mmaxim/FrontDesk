import java.io.File;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.util.StringTokenizer;

/**
 *  Implements a text generator
 *
 *  I used a lot of recursion in this program. I think it leads to 
 *  writing code that is pretty intuituve. I used it a lot more in the 
 *  Trie class.
 *
 *  @author Kwasi Mensah
 *  @see TextGeneratorInterface
 */
public class TextGenerator implements TextGeneratorInterface
{
    /* Private Instance Variables */
    private Trie trie;
    private final int gram_size = 3;
    private int dataSize=0;

    /* Constructor */
    public TextGenerator()
    {
	/* you may use a constructor if you like but you must not modify it
	 * to take any arguments.  you probably don't need a constructor */
	
	//need this for my implementation of getTrie;
	trie=null;
    }

    /* Public Methods */
    public int generateText(File input, File data, File output)
    {
	if(!data.exists())
	    return -1;
	if(!input.exists())
	    return -2;

	BufferedReader in=null; //so Java won't yell about uninitializtion (can;t come out of try w/o bein okay but javac wont tell)
	int words=0; //words to generate for each intital 2 gram
	try{
	    in=new BufferedReader(new FileReader(input));
	    words=Integer.parseInt(in.readLine());
	}
	catch(IOException e){
	    return -3;
	}
	buildTrie(data);
	
	
	BufferedWriter out=null;
	
	try{
	    out=new BufferedWriter(new FileWriter(output));
	    //write the size info
	    String tempWrite="";
	    tempWrite="Level 0: "+trie.sizeLevel0()+
		"\nLevel 1: "+trie.sizeLevel1()+
		"\nLevel 2: "+trie.sizeLevel2()+
		"\nWords: "+dataSize+"\n\n";
	    out.write(tempWrite);
	    out.flush();
	    	    
	    while((tempWrite=generateSentence(in.readLine(),words))!=null){
		out.write(tempWrite+"\n");
		out.flush();
	    }
	    
	    out.flush();
	    out.close();
       	}	
	catch(IOException e){
	    System.out.println(e);
	}

	return 0;
    }

    public int buildTrie(File data)
    {
	if(trie==null) //makes sure getTrie return empty until we've actually called buildTrie
	    trie=new Trie();

	if(data==null||!data.exists()) //error conditions
	    return -1;
	// being able to read strings from a file adapted from javaalmanac.com/egs/java.io/ReadLinesFromFile.html
	try{
	
	    String firstLine=null;
	    String sndLine=null;
	    String trdLine=null;
	    
	    
	    BufferedReader in=new BufferedReader(new FileReader(data));
	   
	    
	    StringTokenizer allTokens=new StringTokenizer(in.readLine());    
	    

	    do{
		
		//transfer all the words up one
		firstLine=sndLine;
		sndLine=trdLine;
		
		//It took me a while to realize hat it would be much faster to just StringTokenize the file one line at a time
		while(allTokens.countTokens()==0){
		    String tempCheck=in.readLine();
		    if(tempCheck==null) //we've reached the end of the file
			return 0;
		    if(!tempCheck.trim().equals(""))// make sure we didnt take in a "word" of whitspace
			allTokens=new StringTokenizer(tempCheck);
		}
		trdLine=allTokens.nextToken();
		
		if(firstLine!=null) //will only be false the 1st two time but we dont want errors with +
		    trie.insertGram(firstLine+" "+sndLine+" "+trdLine);	
		
		dataSize++;
	    }
	    while(true); //do this till you ant read from wile anymore (dealt with insde of loop)       
	
	}
	catch(IOException e){}	
	System.out.println("Done Building Trie");
	return 0;
    }

    public String generateSentence(String s, int words)
    {
	if(words==0) //that's right, setting the stage for recursion  (HELL YEAH!!!)
	    return s;
	
	if(s==null)
	    return null;	

	if(trie==null)
	    return null;
	
	StringTokenizer wordList=new StringTokenizer(s);

	if(wordList.countTokens()<2)
	    return null;

	String fstWord=null;
	String sndWord=null;
	

	//the reason for this is a little complicated.
	//I thought i;d be smart and use the part of the specification that said we only needed to worry about the last two words
	//and would just add the generated word to th end. However, I realised that without this while loop, it would always be O(n)
	//to find the last two. So I added this loop. There are still n loops but only the last 2 amount to anything so I figured
	//it would be fine.
	while(wordList.countTokens()>2)
	    wordList.nextToken();

	//this makes sure we deal with the last two words per specification
	while(wordList.countTokens()!=0){
	    fstWord=sndWord;
	    sndWord=wordList.nextToken();
	}
	
	String stopCheck=trie.generateNext(fstWord+" "+sndWord);
	s+=" "+stopCheck;
	
	if(stopCheck.equals("STOP"))
	    return s;
	return generateSentence(s,words-1);
    }

    public Trie getTrie(){

	return trie;
    }

    public int numWords()
    {
	/* write this method */
	
	if(trie==null)
	    return -1;
	return dataSize;
    }

    public static void main(String[] args){
	File dataFile=null;
	File input=null;
	File output=null;

	TextGenerator test=new TextGenerator();
	if(args.length==3){
	    input=new File(args[0]);
	    dataFile=new File(args[1]);
	    output=new File(args[2]);
	}
	//System.out.println(test.getTrie());
	//test.buildTrie(dataFile);
	//System.out.println(test.trie);
	//System.out.println(test.generateSentence("ox at",10));
	test.generateText(input, dataFile,output);
    }
}

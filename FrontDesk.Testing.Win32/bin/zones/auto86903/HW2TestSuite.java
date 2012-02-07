import java.util.*;
import java.io.*;

import junit.framework.Assert;
import junit.framework.Test;
import junit.framework.TestSuite;
import junit.frontdesk.FrontDeskTestCase;


/**
 * @author Will Haines
 */
public class HW2TestSuite extends FrontDeskTestCase {

    public HW2TestSuite (String name, double fpoints, double epoints, int timelimit) {    
        super(name, fpoints, epoints, timelimit);
    }
    
    public HW2TestSuite () {
        super();
    }
    
    public static Test suite () {    
        
      TestSuite suite= new TestSuite();

      suite.addTest( new HW2TestSuite( "Test: generateText() for a non-existent input file.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateText1(); } 
	  } ); 
      suite.addTest( new HW2TestSuite( "Test: generateText() on an invalid input file.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateText2(); } 
	  } ); 
      suite.addTest( new HW2TestSuite( "Test: generateText() on a non-existent input file.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateText3(); } 
	  } );    
      suite.addTest( new HW2TestSuite( "Test: generateText() on a valid input sequence.", 2.0, 2.0, 5 ) { 
	      public void runTest() 
	      { testGenerateText4(); } 
	  } );    
      suite.addTest( new HW2TestSuite( "Test: buildTrie() on a non-existent input file.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testBuildTrie1(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: buildTrie() on a valid input file.", 2.0, 2.0, 5 ) { 
	      public void runTest() 
	      { testBuildTrie2(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Basic Test: generateSentence() on small input.", 3.0, 3.0, 5 ) { 
	      public void runTest() 
	      { testGenerateSentenceB1(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Basic Test: generateSentence() on medium input.", 3.0, 3.0, 5 ) { 
	      public void runTest() 
	      { testGenerateSentenceB2(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Basic Test: generateSentence() on large input.", 4.0, 4.0, 20 ) { 
	      public void runTest() 
	      { testGenerateSentenceB3(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Basic Test 2: generateSentence() on small input with STOP.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateSentenceB21(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Basic Test 2: generateSentence() on medium input with STOP.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateSentenceB22(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Basic Test 2: generateSentence() on large input with STOP.", 2.0, 2.0, 20 ) { 
	      public void runTest() 
	      { testGenerateSentenceB23(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Comprehensive Test: generateSentence() on small input.", 3.0, 3.0, 5 ) { 
	      public void runTest() 
	      { testGenerateSentenceC1(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Comprehensive Test: generateSentence() on medium input.", 3.0, 3.0, 5 ) { 
	      public void runTest() 
	      { testGenerateSentenceC2(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Comprehensive Test: generateSentence() on large input.", 4.0, 4.0, 20 ) { 
	      public void runTest() 
	      { testGenerateSentenceC3(); } 
	  } ); 
      suite.addTest( new HW2TestSuite( "Test: getTrie() before buildTrie() has been called.", 2.0, 2.0, 5 ) { 
	      public void runTest() 
	      { testGetTrie(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: numWords() on small input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testNumWords1(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: numWords() on medium input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testNumWords2(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: numWords() on large input.", 2.0, 2.0, 20 ) { 
	      public void runTest() 
	      { testNumWords3(); } 
	  } ); 
      suite.addTest( new HW2TestSuite( "Test: insertGram() on invalid input.", 3.0, 3.0, 5 ) { 
	      public void runTest() 
	      { testInsertGram(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on invalid input.", 2.0, 2.0, 20 ) { 
	      public void runTest() 
	      { testGenerateNextInvalid(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on small input.", 2.0, 2.0, 5 ) { 
	      public void runTest() 
	      { testGenerateNext1(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on medium input.", 2.0, 2.0, 5 ) { 
	      public void runTest() 
	      { testGenerateNext2(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on large input.", 3.0, 3.0, 20 ) { 
	      public void runTest() 
	      { testGenerateNext3(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on small input with STOP.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateNext21(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on medium input with STOP.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testGenerateNext22(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: generateNext() on large input with STOP.", 1.0, 1.0, 20 ) { 
	      public void runTest() 
	      { testGenerateNext23(); } 
	  } );      
      suite.addTest( new HW2TestSuite( "Test: sizeLevel0() on small input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testSizeLevel01(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel0() on medium input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testSizeLevel02(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel0() on large input.", 2.0, 1.0, 20 ) { 
	      public void runTest() 
	      { testSizeLevel03(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel1() on small input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testSizeLevel11(); }
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel1() on medium input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testSizeLevel12(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel1() on large input.", 2.0, 1.0, 20 ) { 
	      public void runTest() 
	      { testSizeLevel13(); } 
	  } );      
      suite.addTest( new HW2TestSuite( "Test: sizeLevel2() on small input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testSizeLevel21(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel2() on medium input.", 1.0, 1.0, 5 ) { 
	      public void runTest() 
	      { testSizeLevel22(); } 
	  } );
      suite.addTest( new HW2TestSuite( "Test: sizeLevel2() on large input.", 2.0, 1.0, 20 ) { 
	      public void runTest() 
	      { testSizeLevel23(); } 
	  } );


    
      return suite; 
    } 
 
    private TextGeneratorInterface tGen;
    private TrieInterface trie;
    private File noFile;
    private File invalidInput;
    private File validInput;
    private File wrap;
    private File small;
    private File mid;
    private File big;
    private File output;
    
    protected void setUp () {
	tGen = new TextGenerator();
        trie = new Trie();
	noFile = new File( "foo.txt" );
	invalidInput = new File( "invalid.txt" );
	validInput = new File( "valid.txt" );
	wrap = new File( "wraptext.txt" );
	small = new File( "smalltext.txt" );
	mid = new File( "midtext.txt" );
	big = new File( "bigtext.txt" );
	output = new File( "out.txt" );
    }
    
    protected void tearDown () {
        ;
    }

    public void testGenerateText1 () {
 	Assert.assertEquals( "Fails generateText() for a non-existent input file.", -2, 
			     tGen.generateText( noFile, small, output ) );
    }
    
    public void testGenerateText2 () {
 	Assert.assertEquals( "Fails generateText() for an invalid input file.", -3, 
			     tGen.generateText( invalidInput, small, output ) );
    }
    
    public void testGenerateText3 () {
 	Assert.assertEquals( "Fails generateText() for a non-existent data file.", -1, 
			     tGen.generateText( validInput, noFile, output ) );
    }
    
    public void testGenerateText4 () {
 	Assert.assertEquals( "Fails generateText() for a valid input sequence.", 0, 
			     tGen.generateText( validInput, small, output ) );

	tGen.buildTrie( wrap );

	String answer = ( tGen.generateSentence( "c d", 3 ) ).trim();
	Assert.assertTrue( "Fails generateText() for a valid input sequence due to incorrect parsing of line wraps.",
			   answer.equals( "c d e f g" ) );
    }
    
    public void testBuildTrie1() {
	Assert.assertEquals( "Fails buildTrie() for a non-existent input file.", -1,
			     tGen.buildTrie( noFile ) );
    }

    public void testBuildTrie2() {
	Assert.assertEquals( "Fails buildTrie() for a valid input file.", 0,
			     tGen.buildTrie( small ) );
    }

    public void testGenerateSentenceB1() {
	tGen.buildTrie( small );
	
	String answer = ( tGen.generateSentence( "a b", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"a b\", 3 ); expected: (\"a b c d e\"), but was: (\""
		+ answer + "\").", answer.equals( "a b c d e" ) );
	answer = ( tGen.generateSentence( "c d", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"d a\", 3 ); expected: (\"c d e f g\"), but was: (\""
		+ answer + "\").", answer.equals( "c d e f g" ) );
	answer = ( tGen.generateSentence( "f g", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"f g\", 3 ); expected: (\"f g h i j\"), but was: (\""
		+ answer + "\").", answer.equals( "f g h i j" ) );
    }

    public void testGenerateSentenceB2() {
	tGen.buildTrie( mid );

	String answer = ( tGen.generateSentence( "b d", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"b d\", 3 ); expected: (\"b d d a c\"), but was: (\""
		+ answer + "\").", answer.equals( "b d d a c" ) );
	answer = ( tGen.generateSentence( "a c", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"a c\", 3 ); expected: (\"a c a a b\"), but was: (\""
		+ answer + "\").", answer.equals( "a c a a b" ) );
	answer = ( tGen.generateSentence( "d a", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"d a\", 3 ); expected: (\"d a c a a\"), but was: (\""
		+ answer + "\").", answer.equals( "d a c a a" ) );
    }

   public void testGenerateSentenceB3() {
	tGen.buildTrie( big );

	String answer = ( tGen.generateSentence( "Tom Sawyer", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"Tom Sawyer\", 3 ); expected: (\"Tom Sawyer came forward with\"), but was: (\""
		+ answer + "\").", answer.equals( "Tom Sawyer came forward with" ) );
	answer = ( tGen.generateSentence( "old lady", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"old lady\", 3 ); expected: (\"old lady came back and\"), but was: (\""
		+ answer + "\").", answer.equals( "old lady came back and" ) );
	answer = ( tGen.generateSentence( "to the", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"to the\", 3 ); expected: (\"to the wall, and die\"), but was: (\""
		+ answer + "\").", answer.equals( "to the wall, and die" ) );
    }

    public void testGenerateSentenceB21() {
	tGen.buildTrie( small );
	
	String answer = ( tGen.generateSentence( "a c", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"a c\", 3 ); expected: (\"a c STOP\"), but was: (\""
		+ answer + "\").", answer.equals( "a c STOP" ) );
	answer = ( tGen.generateSentence( "x x", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"x x\", 3 ); expected: (\"x x STOP\"), but was: (\""
		+ answer + "\").", answer.equals( "x x STOP" ) );
    }

    public void testGenerateSentenceB22() {
	tGen.buildTrie( mid );

	String answer = ( tGen.generateSentence( "a e", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"a e\", 3 ); expected: (\"a e STOP\"), but was: (\""
		+ answer + "\").", answer.equals( "a e STOP" ) );
	answer = ( tGen.generateSentence( "x a", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"x a\", 3 ); expected: (\"x a STOP\"), but was: (\""
		+ answer + "\").", answer.equals( "x a STOP" ) );
    }

   public void testGenerateSentenceB23() {
	tGen.buildTrie( big );

	String answer = ( tGen.generateSentence( "go fame", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"go fame\", 3 ); expected: (\"go fame STOP\"), but was: (\""
		+ answer + "\").", answer.equals( "go fame STOP" ) );
	answer = ( tGen.generateSentence( "Levon Helm", 3 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"Levon Helm\", 3 ); expected: (\"Levon Helm STOP\"), but was: (\""
		+ answer + "\").", answer.equals( "Levon Helm STOP" ) );
    }

    public void testGenerateSentenceC1() {

	String answer = ( tGen.generateSentence( "c d", 3 ) );
	Assert.assertNull( "Fails generateSentence( \"c d\", 3 ) when trie is empty; expected: (null), but was: (\""
			   + answer + "\").", answer );

	tGen.buildTrie( small );	

	answer = ( tGen.generateSentence( "f g", -5 ) );
	Assert.assertNull( "Fails generateSentence( \"f g\", -5 ); expected: (null), but was: (\""
		+ answer + "\").", answer );
	answer = ( tGen.generateSentence( "a", 3 ) );
	Assert.assertNull( "Fails generateSentence( \"a\", 3 ); expected: (null), but was: (\""
		+ answer + "\").", answer );
	answer = ( tGen.generateSentence( "g h", 10 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"g h\", 10 ); expected: (\"g h i j k l m n o p q r\"), but was: (\""
		+ answer + "\").", answer.equals( "g h i j k l m n o p q r" ) );
	answer = ( tGen.generateSentence( "g h", 1 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"g h\", 1 ); expected: (\"g h i\"), but was: (\""
		+ answer + "\").", answer.equals( "g h i" ) );
    }

    public void testGenerateSentenceC2() {

	String answer = ( tGen.generateSentence( "b d", 3 ) );
	Assert.assertNull( "Fails generateSentence( \"b d\", 3 ) when trie is empty; expected: (null), but was: (\""
		+ answer + "\").", answer );

	tGen.buildTrie( mid );

	answer = ( tGen.generateSentence( "a c", -5 ) );
	Assert.assertNull( "Fails generateSentence( \"a c\", -5 ); expected: (null), but was: (\""
		+ answer + "\").", answer );
	answer = ( tGen.generateSentence( "d", 3 ) );
	Assert.assertNull( "Fails generateSentence( \"d\", 3 ); expected: (null), but was: (\""
		+ answer + "\").", answer );
	answer = ( tGen.generateSentence( "a c", 10 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"a c\", 10 ); expected: (\"a c a a b c a a b c a a\"), but was: (\""
		+ answer + "\").", answer.equals( "a c a a b c a a b c a a" ) );
	answer = ( tGen.generateSentence( "a c", 1 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"a c\", 1 ); expected: (\"a c a\"), but was: (\""
		+ answer + "\").", answer.equals( "a c a" ) );
    }

   public void testGenerateSentenceC3() {

	String answer = ( tGen.generateSentence( "Tom Sawyer", 3 ) );
	Assert.assertNull( "Fails generateSentence( \"Tom Sawyer\", 3 ); expected: (null), but was: (\""
		+ answer + "\").", answer);

	tGen.buildTrie( big );

	answer = ( tGen.generateSentence( "old lady", -5 ) );
	Assert.assertNull( "Fails generateSentence( \"old lady\", -5 ); expected: (null), but was: (\""
		+ answer + "\").", answer );
	answer = ( tGen.generateSentence( "to", 3 ) );
	Assert.assertNull( "Fails generateSentence( \"to\", 3 ); expected: (null), but was: (\""
		+ answer + "\").", answer );
	answer = ( tGen.generateSentence( "Tom Sawyer", 10 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"Tom Sawyer\", 10 ); expected: (\"Tom Sawyer came forward with nine yellow tickets, but none had enough\"), but was: (\""
		+ answer + "\").", answer.equals( "Tom Sawyer came forward with nine yellow tickets, but none had enough" ) );
	answer = ( tGen.generateSentence( "Tom Sawyer", 1 ) ).trim();
	Assert.assertTrue( "Fails generateSentence( \"Tom Sawyer\", 1 ); expected: (\"Tom Sawyer came\"), but was: (\""
		+ answer + "\").", answer.equals( "Tom Sawyer came" ) );
    }

    public void testGetTrie() {
	
	tGen = new TextGenerator();
	TrieInterface answer = tGen.getTrie();
	Assert.assertNull( "Fails getTrie() before buildTrie() has been called; expected: (null), but was (" + answer + ").", answer );
    }

    public void testNumWords1() {
  
	tGen.buildTrie( small );
	trie = tGen.getTrie();
       
	Assert.assertEquals( "Fails numWords().", 26, tGen.numWords() );
    }

    public void testNumWords2() {
  
	tGen.buildTrie( mid );
	trie = tGen.getTrie();
       
	Assert.assertEquals( "Fails numWords().", 50, tGen.numWords() );
    }

    public void testNumWords3() {
  
	tGen.buildTrie( big );
	trie = tGen.getTrie();
       
	Assert.assertEquals( "Fails numWords().", 19535, tGen.numWords() );
    }

    public void testInsertGram() {
	
	tGen.buildTrie( small );
	trie = tGen.getTrie();
	
	Assert.assertEquals( "Fails insertGram() on invalid input.", -1, trie.insertGram( "a b c d" ) );
    }

    public void testGenerateNextInvalid() {

	tGen.buildTrie( mid );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"b d d\").", null, trie.generateNext( "b d d" ) );
    }

    public void testGenerateNext1() {

	tGen.buildTrie( small );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"a b\").", "c", trie.generateNext( "a b" ) );
	Assert.assertEquals( "Fails generateNext(\"e f\").", "g", trie.generateNext( "e f" ) );
    }

    public void testGenerateNext2() {

	tGen.buildTrie( mid );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"b d\").", "d", trie.generateNext( "b d" ) );
	Assert.assertEquals( "Fails generateNext(\"c a\").", "a", trie.generateNext( "c a" ) );
    }

    public void testGenerateNext3() {

	tGen.buildTrie( big );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"old lady\").", "came", trie.generateNext( "old lady" ) );
	Assert.assertEquals( "Fails generateNext(\"Sherwood Forest\").", "than", trie.generateNext( "Sherwood Forest" ) );
    }

    public void testGenerateNext21() {

	tGen.buildTrie( small );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"b a\").", "STOP", trie.generateNext( "b a" ) );
	Assert.assertEquals( "Fails generateNext(\"x x\").", "STOP", trie.generateNext( "x x" ) );
    }

    public void testGenerateNext22() {

	tGen.buildTrie( mid );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"a e\").", "STOP", trie.generateNext( "a e" ) );
	Assert.assertEquals( "Fails generateNext(\"x a\").", "STOP", trie.generateNext( "x a" ) );
    }

    public void testGenerateNext23() {

	tGen.buildTrie( big );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails generateNext(\"go fame\").", "STOP", trie.generateNext( "go fame" ) );
	Assert.assertEquals( "Fails generateNext(\"Levon Helm\").", "STOP", trie.generateNext( "Levon Helm" ) );
    }

    public void testSizeLevel01() {

	tGen.buildTrie( small );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel0().", 24, trie.sizeLevel0() );		
    }

    public void testSizeLevel02() {

	tGen.buildTrie( mid );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel0().", 5, trie.sizeLevel0() );		
    }

    public void testSizeLevel03() {

	tGen.buildTrie( big );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel0().", 5321, trie.sizeLevel0() );		
    }

    public void testSizeLevel11() {

	tGen.buildTrie( small );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel1().", 24, trie.sizeLevel1() );		
    }

    public void testSizeLevel12() {

	tGen.buildTrie( mid );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel1().", 18, trie.sizeLevel1() );		
    }

    public void testSizeLevel13() {

	tGen.buildTrie( big );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel1().", 15041, trie.sizeLevel1() );		
    }

    public void testSizeLevel21() {

	tGen.buildTrie( small );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel2().", 24, trie.sizeLevel2() );		
    }

    public void testSizeLevel22() {

	tGen.buildTrie( mid );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel2().", 36, trie.sizeLevel2() );		
    }

    public void testSizeLevel23() {

	tGen.buildTrie( big );
	trie = tGen.getTrie();

	Assert.assertEquals( "Fails sizeLevel2().", 18860, trie.sizeLevel2() );		
    }
}

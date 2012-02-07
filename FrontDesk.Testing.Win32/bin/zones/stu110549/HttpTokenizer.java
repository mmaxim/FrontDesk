/**
 *
 * A simple stream-based parser for web pages.
 *
 *
 * The possible tokens are as follows:
 *
 * HT_EOF: The end of file
 * HT_NUMBER: A number, converted to a double
 * HT_WORD: A word, converted to all lowercase
 * HT_STRING: A quoted string
 * HT_TAGOPEN: A "<" character
 * HT_TAGCLOSE: A ">" character
 * HT_EQUALS: A "=" character
 * HT_SLASH: A "/" character
 * HT_DASH: A "-" character
 * HT_BANG: A "!" character
 * HT_A: The keyword "a"
 * HT_HREF: The keyword "href"
 * HT_IMG: The keyword "img"
 *
 * @author Peter Lee
 *****************************************************************************/

import java.util.*;
import java.io.*;

public class HttpTokenizer
{
    /** A constant indicating the end of the web document has been reached. */
	public static final int HT_EOF = 0;
	/** A constant indicating a number token has been read. */
	public static final int HT_NUMBER = 1;
	/** A constant indicating a word token has been read. */
	public static final int HT_WORD = 2;
	/** A constant indicating a string token has been read. */
	public static final int HT_STRING = 3;
	/** A constant indicating a '<' has been read. */
	public static final int HT_TAGOPEN = 4;
	/** A constant indicating a '>' has been read. */
	public static final int HT_TAGCLOSE = 5;
	/** A constant indicating a '=' has been read. */
	public static final int HT_EQUALS = 6;
	/** A constant indicating a '/' has been read. */
	public static final int HT_SLASH = 7;
	/** A constant indicating a '-' has been read. */
	public static final int HT_DASH = 8;
	/** A constant indicating a '!' has been read. */
	public static final int HT_BANG = 9;
	/** A constant indicating an "a" has been read. */
	public static final int HT_A = 10;
	/** A constant indicating an "href" has been read. */
	public static final int HT_HREF = 11;
	/** A constant indicating an "img" has been read. */
	public static final int HT_IMG = 12;
	/** A constant indicating an "src" has been read. */
	public static final int HT_SRC = 13;

	/** If the current token is a word or string, this field gives the
	string. */
	public String sval;
	/** If the current token is a number, this field contains the value
	of that number. */
	public double nval;

	// The stream tokenizer
	private StreamTokenizer tokens;

	// Each word returned by the stream tokenizer must be further
	// parsed.  This is done by creating a string tokenizer for
	// each word.
	private StringTokenizer word;


	/** Create an HTTP tokenizer, given a Reader for the web page. */
	public HttpTokenizer (Reader page) throws IOException
	{
		// Create a stream tokenizer
		tokens = new StreamTokenizer(page);

		// Set up the appropriate defaults
		tokens.eolIsSignificant(false);
		tokens.lowerCaseMode(true);
		tokens.wordChars('<','<');
		tokens.wordChars('>','>');
		tokens.wordChars('/','/');
		tokens.wordChars('=','=');
		tokens.wordChars('@','@');
		tokens.wordChars('!','!');
		tokens.wordChars('-','-');
		tokens.ordinaryChar('.');
		tokens.ordinaryChar('?');
	}

	/** Parses the next token from the web page.
	@return The code of the next token.
	*/
	public int nextToken() throws IOException
	{
		// First, check if the last word we read still has more tokens in it
		if (word != null && word.hasMoreTokens())
		{
			sval = word.nextToken();
			if (sval.equals("<")) return HT_TAGOPEN;
			if (sval.equals(">")) return HT_TAGCLOSE;
			if (sval.equals("=")) return HT_EQUALS;
			if (sval.equals("/")) return HT_SLASH;
			if (sval.equals("-")) return HT_DASH;
			if (sval.equals("!")) return HT_BANG;
			if (sval.equals("a")) return HT_A;
			if (sval.equals("href")) return HT_HREF;
			if (sval.equals("img")) return HT_IMG;
			if (sval.equals("src")) return HT_SRC;
			return HT_WORD;
		}

		// Otherwise, read in a new token from the stream
		word = null;
		int t = tokens.nextToken();

		if (t == StreamTokenizer.TT_WORD)
		{
			word = new StringTokenizer(tokens.sval, "<>/=!-", true);
			return this.nextToken();
		}
		if (t == StreamTokenizer.TT_NUMBER)
		{
			nval = tokens.nval;
			return HT_NUMBER;
		}
		if (t == 34)
		{ // The magic number for string tokens
			sval = tokens.sval;
			return HT_STRING;
		}
		if (t == StreamTokenizer.TT_EOF)
			return HT_EOF;

		// If nothing interesting found, then go for the next token
		return this.nextToken();
	}
}

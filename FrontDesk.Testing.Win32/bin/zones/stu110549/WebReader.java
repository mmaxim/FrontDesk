/**
 ******************************************************************************
 *                    HOMEWORK 4, 15-211
 ******************************************************************************
 *                    Part 1: The Web Reader
 ******************************************************************************
 *
 * This is a simple parser for web pages, based on the use of a finite state
 * machine.
 *
 *
 * It takes the address of a WWW page and reads it, printing all of the
 * keywords, numbers, hyperlinks and image filenames that appear on that page.
 *
 * To run this program from the command line, type the following:
 *
 *     % java WebReader <url>
 *
 * where <url> is the URL of a web page to read.
 *
 * @author  P. Lee and V. Adamchik
 * @date		12/03/05
 * @see		PageLexer
 * @see		HttpTokenizer
 * @see		ElementList
 * @see		PageElement
 *****************************************************************************

                 YOU DO NOT NEED TO IMPLEMENT ANYTHING IN THIS PART

 *****************************************************************************/
import java.io.*;
import java.net.*;
import java.util.*;

public class WebReader
{
	public static String baseURL; //we use this for fixing html links
	private PageLexer pageElements;      //page elements

	public static void main (String[] args)
	{
		if (args.length != 1)
		{
			System.out.println("Usage: WebReader <url>");
			return;
		}
		else
			new WebReader(args[0]);
	}

	public WebReader(String url)
	{
		pageElements = new PageLexer();
		BufferedReader reader = null;
		try
		{
			baseURL = getHost(url);

			URL u = new URL(url);
			URLConnection c = u.openConnection();
			String t = c.getContentType();

			if (t == null || !t.startsWith("text/html")) {
				System.out.println("t is null ");//my own line of test
				return;}

			reader = new BufferedReader(new InputStreamReader(c.getInputStream()));
System.out.println("reader is reading");//my test
			// Parse the page into its elements
			pageElements = new PageLexer(reader);

			// DEBUG: Prints page elements in groups
			//printElements(pageElements);
		}
		catch (IOException e)
		{
			System.out.println(url);
			System.out.println("Bad file or URL specification");
		}
		finally
		{
			try
			{
				if(reader != null) reader.close();
			System.out.println("reader close successfuly");//my test
			}
			catch (IOException e)
			{
				System.out.println(e);
			}
		}
	}

 /**
	*  Returns a list of HTML links
	*/
	public Iterator getLinks()
	{
		return pageElements.getNode("PageHref");
	}

 /**
	*  Returns a list of words
	*/
	public Iterator getWords()
	{
		return pageElements.getNode("PageWord");
	}

 /**
	*  Reduces the URL name to include only the subdirectory information
	*  For example
	*		http://money.cnn.com/index.html
	*  will be reduced to
	*     http://money.cnn.com/
	*/
	public String getHost(String url)
	{
		int m = url.indexOf(".htm");
		int n = url.lastIndexOf('/', url.length()) + 1;

		if (m == -1)
			return n == url.length()?url:url+"/";
		else
			return n == 6?url:url.substring(0,n);
	}

 /**
	*  This is for debugging
	*/
	private void printElements(PageLexer nodes)
	{
		Iterator itrWords = nodes.getNode("PageWord");
		Iterator itrHttps = nodes.getNode("PageHref");
		Iterator itrImgs  = nodes.getNode("PageImg");
		Iterator itrNums  = nodes.getNode("PageNum");

		// Print out the tokens

		int count = 0;
		while (itrWords.hasNext())
		{
			count++;
			System.out.println("word: "+ itrWords.next());
		}
		
		/*
		while (itrHttps.hasNext())
		{
			count++;
			System.out.println("link: "+ itrHttps.next());
		}
		*/
		
		
		while (itrImgs.hasNext())
		{
			count++;
			System.out.println("image: "+ itrImgs.next());
		}
		while (itrNums.hasNext())
		{
			count++;
			System.out.println("num: "+ itrNums.next());
		}
	
		while (itrHttps.hasNext())
		{
			count++;
			System.out.println("link: "+ itrHttps.next());
		}	
	
	
	
		System.out.println();
		System.out.println(count + " total page elements retrieved.");
	}



}

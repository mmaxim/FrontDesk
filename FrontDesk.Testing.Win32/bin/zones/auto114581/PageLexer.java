/**
 *
 * A lexical analyzer for web documents, based on a finite-state
 * machine.
 *
 *
 *
 * @author Peter Lee and V. Adamchik
 * @date   12/03/05
 *****************************************************************************

                 YOU DO NOT NEED TO IMPLEMENT ANYTHING THIS FILE

 *****************************************************************************/

import java.io.*;
import java.util.*;
import java.net.*;

public class PageLexer
{
    // The PageElements that were found in this web page
    private PageElementList quants;

    // The current tokenizer
    private HttpTokenizer tokenStream;

 /**
	* The state-transition table.  A transition to -1 means halt.
	* delta[state][token] yields the next state of the finite-state machine.
	*
	* Note that this is a very simple FSM.  It is possible that better
	* web indexing could be done by modifying this FSM.
	*/
    private int delta[][] = {
  //EOF NUM WRD STR  <   >   =   /   -   !   A  HRF IMG SRC
	{-1,  3,  1,  1,  2,  0,  0,  0,  0,  0,  0,  0,  0,  0},  // state 0
	{-1,  3,  1,  1,  2,  0,  0,  0,  0,  0,  0,  0,  0,  0},  // state 1
	{-1,  5,  5,  5,  5,  6,  5,  5,  5,  5,  4,  5, 10,  5},  // state 2
	{-1,  3,  1,  1,  2,  0,  0,  0,  0,  0,  0,  0,  0,  0},  // state 3
	{-1,  5,  5,  5,  5,  6,  5,  5,  5,  5,  5,  7,  5,  5},  // state 4
	{-1,  5,  5,  5,  5,  6,  5,  5,  5,  5,  5,  5,  5,  5},  // state 5
	{-1,  3,  1,  1,  2,  0,  0,  0,  0,  0,  0,  0,  0,  0},  // state 6
	{-1,  5,  5,  5,  5,  5,  8,  5,  5,  5,  5,  5,  5,  5},  // state 7
	{-1,  5,  5,  9,  5,  5,  5,  5,  5,  5,  5,  5,  5,  5},  // state 8
	{-1,  5,  5,  5,  5,  6,  5,  5,  5,  5,  5,  5,  5,  5},  // state 9
	{-1,  5,  5,  5,  5,  6,  5,  5,  5,  5,  5,  5,  5,  11}, // state 10
	{-1,  5,  5,  5,  5,  6,  12, 5,  5,  5,  5,  5,  5,  11}, // state 11
	{-1,  5,  5,  13, 5,  6,  12, 5,  5,  5,  5,  5,  5,  11}, // state 12
	{-1,  5,  5,  13, 5,  6,  12, 5,  5,  5,  5,  5,  5,  11}, // state 13
	};

 /**
	* Creates an empty lexer.
	*
	*/
	public PageLexer ()
	{
		quants = new PageElementList();
	}

 /**
	* Creates a new web page lexer.
	*
	*/
	public PageLexer (Reader page) throws IOException
	{
		quants = new PageElementList();
		int state = 0;

		// The tokenizer for the given web page
		tokenStream = new HttpTokenizer(page);
		int token;

		while((token = tokenStream.nextToken()) != 0)
		{
			state = delta[state][token];
			if (state >= 0)
				action(state);
			else break;
		}
	}

	private void action (int state)
	{
		switch (state)
		{
			case 0:  break;
			case 1:  quants.add(tokenStream.sval, "PageWord");break;
			case 2:  break;
			case 3:  quants.add(tokenStream.nval+"", "PageNum"); break;
			case 4:  break;
			case 5:  break;
			case 6:  break;
			case 7:  break;
			case 8:  break;
			case 9:  quants.add(tokenStream.sval, "PageHref"); break;
			case 10: break;
			case 11: break;
			case 12: break;
			case 13: quants.add(tokenStream.sval, "PageImg"); break;
		}
	}

 /**
 	*  Returns an iterator over a collection of nodes described
 	*  by a specified tag name
	*
	*/
	public Iterator getNode(String tagName)
	{
		return quants.iterator(tagName);
	}
}

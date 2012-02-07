/**
 ******************************************************************************
 *                    HOMEWORK 4, 15-211
 ******************************************************************************
 *
 * A suggested interface for a class of web-indexing objects.
 *
 *
 * @see		WebIndexer
 *****************************************************************************/

import java.io.*;
import java.net.*;
import java.util.*;

public interface Dictionary
{
 /** Add the given web page to the dictionary.
	*
	* @param url The web page to add to the index
	* @param keywords The keyword that are in the web page
	*/
	public void addPage(String url, Iterator keyword);

 /** Produce a printable representation of the index.
	*
	* @return a String representation of the index structure
	*/
	public String printTree();

 /** Retrieve all of the web pages that contain the given keyword.
	*
	* @param keyword The keyword to search on
	* @return An iterator of the web pages that match.
	*/
	public Iterator retrievePages(String keyword);

 /** Retrieve all of the web pages that contain all of the given keywords.
	*
	* @param keywords The keywords to search on
	* @return An iterator of the web pages that match.
	*/
	public Iterator retrievePages(String[] keywords);

 /** Save the index to a file.
	*
	* @param stream The stream to write the index
	*/
	public void save(String fileName);
}

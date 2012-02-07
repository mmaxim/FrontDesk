
/**
 *  A hyperlink in a web page.
 *
 *  @author V. Adamchik
 *  @date		12/03/05
 */
import java.net.*;

public class PageHref implements PageElement
{
	private URL href;

	public void setData (String str) throws MalformedURLException
	{
			if(str.startsWith("http://"))
				href = new URL(str);
			else
			if(str.startsWith("mailto:"))
				throw new MalformedURLException();
			else
				href = new URL(WebReader.baseURL + str);
	}

	public String toString ()
	{
		return href.toString();
	}

}

/**
 *  A image src in a web page.
 *
 *  @author V. Adamchik
 *  @date   12/03/05
 */

public class PageImg implements PageElement
{
	private String data;

	public void setData(String str)
	{
		data = str;
	}

	public String toString ()
	{
		return data;
	}
}

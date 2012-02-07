/** A img src in a web page.
 *
 */
public class PageWord implements PageElement
{
	private String data;

	public void setData(String str) throws Exception
	{
		try
		{
			Double.parseDouble(str);
			throw new Exception();
		}
		catch(NumberFormatException e)
		{
			data = str;
		}
 	}

	public String toString ()
	{
		return data;
	}
}
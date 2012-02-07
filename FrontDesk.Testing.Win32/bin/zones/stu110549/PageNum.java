/**
 *  A number in a web page.
 *
 *  @author V. Adamchik
 *  @date   12/03/05
 */

public class PageNum implements PageElement
{
	private double data;

	public void setData(String str) throws Exception
	{
		try
		{
			data = Double.parseDouble(str);
		}
		catch(NumberFormatException e)
		{
			//data = str;
		}
	}

	public String toString ()
	{
		return data+"";
	}
}
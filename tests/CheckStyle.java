//Mike Maxim
//CheckStyle adapter

import java.io.*;

import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;

public class CheckStyle {

    public CheckStyle() { }

    private static final String TEMPXML = "__csxml__.xml";

    public void go() throws Exception {
	Runtime.getRuntime().exec(
		"java com.puppycrawl.tools.checkstyle.Main -c checks.xml -r . -f xml -o"
		+ TEMPXML).waitFor();    
	
	transform(System.out);
    }

    private void transform(PrintStream out) throws Exception {
	Transformer transformer = 
	    TransformerFactory.newInstance().newTransformer(
					       new StreamSource("checksubj.xslt"));
	transformer.transform(new StreamSource(TEMPXML), new StreamResult(out));
    }

    public static void main(String[] args) throws Exception {
	new CheckStyle().go();
    }
}

import java.util.Arrays;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Iterator;

/**
 *  Implements a trie node
 *
 *  @author Colin Rothwell
 */
public class Node implements Comparator
{
    private HashMap children;
    private String value;
    private int frequency;

    public Node()
    {
        this("");
    }

    public Node(String value)
    {
        this(value, 0);
    }

    public Node(String value, int frequency)
    {
        children = new HashMap();
        this.value = value;
        this.frequency = frequency;
    }

    public Node insert(String word)
    {
        Node child;
        if(!(children.containsKey(word))){
            child = new Node(word);
            children.put(word, child);
        }
        else{
            child = (Node)(children.get(word));
        }
        return child;
    }

    public Node get(String word)
    {
        if(children.containsKey(word)){
            return (Node)(children.get(word));
        }
        return null;
    }

    public String mostFrequent()
    {
        Object[] values = children.values().toArray();
        Arrays.sort(values, new Node());
        return ((Node)values[0]).value();
    }


    public Iterator children()
    {
        return children.values().iterator();
    }


    public int size()
    {
        return (children.size());
    }

    public int increment()
    {
        return (++frequency);
    }

    public String value()
    {
        return value;
    }

    public int frequency()
    {
        return frequency;
    }

    public int compareTo(Object o)
    {
        Node n = (Node)o;
        int f1 = this.frequency;
        int f2 = n.frequency();
	
        if(f1 == f2){
            return this.value.compareTo(n.value());
        }
        return f2 - f1;
    }

    public int compare(Object o1, Object o2)
    {
        return ((Node)o1).compareTo((Node)o2);
    }

    public String toString()
    {
        return (value + " " + frequency);
    }
}
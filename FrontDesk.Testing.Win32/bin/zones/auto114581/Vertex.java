/**
 *  This class represents an auxiliary information for an each node, such that
 *  a) label
 *  b) in-degree
 *
 *  @author  Victor Adamchik
 *  @date: Nov. 18, 2003
 */

public class Vertex implements Comparable
{
   private int degree;
   private String label;

   public Vertex(int d, String l)
   {
      degree = d;
      label = l;
   }

   public Vertex()
   {
      degree = 0;
      label = null;
   }

   public int compareTo(Object x)
   {
      return ((Vertex) x).degree - this.degree;
   }

   public String getLabel()
   {
      return label;
   }

   public void setLabel(String s)
   {
      label = s;
   }

   public int getDegree()
   {
      return degree;
   }

   public void increaseDegree()
   {
      degree++;
   }

   public String toString()
   {
      return degree + ", " + label;
   }
}

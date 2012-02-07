import java.util.ArrayList;

public class TesterMove implements Move, Cloneable
{
    private int value;
    private int evaluated;
    private TesterMove left;
    private TesterMove right;
    
    public TesterMove(int value, TesterMove left, TesterMove right)
    {
        this.value = value;
        this.left = left;
        this.right = right;
        this.evaluated = 0;
    }
    
    public TesterMove(TesterMove left, TesterMove right)
    {
        this(0, left, right);
    }
    
    public TesterMove(int value)
    {
        this(value, null, null);
    }
    
    public TesterMove(int value, TesterMove left)
    {
        this(value, left, null);
    }
    
    public int evaluate()
    { 
        evaluated++;
        return value;
    }
    
    public void reset()
    {
        evaluated = 0;
        
        if(left != null){
            left.reset();
        }
        if(right != null){
            right.reset();
        }
    }
    
    public void evaluates(ArrayList values, int level)
    {
        if(level == 0){
            values.add(new Integer(evaluated));
            return;
        }
        
        if(left != null){
            left.evaluates(values, level-1);
        }
        
        if(right != null){
            right.evaluates(values, level-1);
        }
    }
    
    public void value(int value) { this.value = value; }
    
    public TesterMove left() { return left; }
    
    public void left(TesterMove left) { this.left = left; }
    
    public TesterMove right() { return right; }
    
    public void right(TesterMove right) { this.right = right; }
    
    public Object clone(){ return this; }
}

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

public class TesterGame implements GameState
{
    private TesterMove root;
    private TesterMove curr;
    private boolean whitesTurn;
    public int[] leaves;
    public int[] evaluates;
    public int score;
    
    private static TesterMove createRoot(LinkedList queue)
    {
        while(queue.size() > 1){
            queue.addLast(new TesterMove((TesterMove)queue.removeFirst(), (TesterMove)queue.removeFirst()));
        }
        return (TesterMove)queue.removeFirst();
    }
    
    public TesterGame(int[] leaves, int[] evaluates, int score)
    {
        this.leaves = leaves;
        this.evaluates = evaluates;
        this.score = score;
        
        LinkedList queue = new LinkedList();
        for(int a=0; a<leaves.length; a++){
            queue.addLast(new TesterMove(leaves[a]));
        }
        root = createRoot(queue);
        curr = root;
        whitesTurn = true;
    }

    public TesterGame(TesterGame game)
    {
        this.root = game.root;
        this.curr = game.curr;
        this.whitesTurn = game.whitesTurn;
        this.leaves = game.leaves;
        this.evaluates = game.evaluates;
        this.score = game.score;
    }
    
    public TesterGame(TesterGame game, TesterMove move)
    {
        this(game);
        applyMove(move);
    }   
    
    public static String string(int[] arr)
    {
        String ret = "(";
        for(int i=0; i<arr.length; i++){
            if(i > 0)
                ret += ",";
            ret += arr[i];
        }
        ret += ")";
        return ret;
    }
    
    public static String string(double[] arr)
    {
        String ret = "(";
        for(int i=0; i<arr.length; i++){
            if(i > 0)
                ret += ",";
            ret += arr[i];
        }
        ret += ")";
        return ret;
    }
    
    public double value()
    {
        return (double)curr.evaluate();
    }
    
    public int[] check(int level)
    {
        ArrayList values = new ArrayList();
        root.evaluates(values, level);
        
        int[] ret = new int[values.size()];
        for(int i=0; i<values.size(); i++){
            ret[i] = ((Integer)values.get(i)).intValue();

        }
        return ret;
    }
    
    public void reset()
    {
        root.reset();
    }
    
    private void applyMove(TesterMove move)
    {
        curr = move;
        whitesTurn = !whitesTurn;
    }
    
    public boolean legalMove(Move m)
    {
        TesterMove move = (TesterMove)m;
        return (move == curr.left() || move == curr.right());
    }

    public List getAllLegalMoves()
    {
        List moves = new ArrayList();
        if(curr.left() != null)
            moves.add(curr.left());
        if(curr.right() != null)
            moves.add(curr.right());
        return moves;
    }

    public GameState getResultingGameState(Move m)
    {
        if(legalMove(m)){
            TesterMove move = (TesterMove)m;
            return new TesterGame(this, move);
        }
        return null;
    }
    
    public boolean isWhitesTurn()
    {
        return whitesTurn;
    }
}

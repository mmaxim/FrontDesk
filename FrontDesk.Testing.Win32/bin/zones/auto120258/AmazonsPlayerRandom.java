import java.util.List;

public class AmazonsPlayerRandom implements AmazonsPlayer, Cloneable {
    public void generateMove (GameState gs, BestMoveSoFar move) {
        List c = gs.getAllLegalMoves();
        if (c.isEmpty())
            return;
        int randEleNumber = (int)(Math.random()*c.size());
        Move m = (Move)c.toArray()[randEleNumber];
        move.set(m);
    }
    
    public Object clone () {
        try {
            return super.clone();
        } catch (CloneNotSupportedException e) {
            e.printStackTrace();
            return null;
        }
    }
}


public class AmazonsMove implements Cloneable, Move {
    protected int queenOldX;
    protected int queenOldY;
    protected int queenNewX;
    protected int queenNewY;
    protected int arrowX;
    protected int arrowY;
    
    public AmazonsMove (int queenOldX, int queenOldY,
                 int queenNewX, int queenNewY,
                 int arrowX, int arrowY) {
        this.queenOldX = queenOldX;
        this.queenOldY = queenOldY;
        this.queenNewX = queenNewX;
        this.queenNewY = queenNewY;
        this.arrowX = arrowX;
        this.arrowY = arrowY;
    }
    
    public Object clone () {
        try {
            return super.clone();
        } catch (CloneNotSupportedException e) {
            throw new InternalError("Not Cloneable");
        }
    }
    
    public String toString () {
        return "[" + queenOldX + "," + queenOldY + "] " + 
            "[" + queenNewX + "," + queenNewY + "] " +
            "[" + arrowX + "," + arrowY + "]";
    }

    public int getArrowX () {
        return arrowX;
    }
    

    public int getArrowY () {
        return arrowY;
    }
    

    public int getQueenNewX () {
        return queenNewX;
    }
    

    public int getQueenNewY () {
        return queenNewY;
    }
    

    public int getQueenOldX () {
        return queenOldX;
    }
    

    public int getQueenOldY () {
        return queenOldY;
    }
}

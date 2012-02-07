import java.util.Arrays;

import junit.framework.Assert;
import junit.framework.Test;
import junit.framework.TestSuite;
import junit.frontdesk.FrontDeskTestCase;

public class HW6Comptest extends FrontDeskTestCase
{
    private static final int GAMES = 10;
    private static final int DEPTH = 4;
    
    private static final int[][] leaves = 
    {{21,2,20,22,5,13,3,5,12,5,5,30,21,22,12,2},
     {30,22,20,7,16,4,28,9,5,21,4,22,23,23,29,7},
     {27,9,5,10,26,1,0,11,4,6,26,15,29,6,23,31},
     {27,27,5,9,14,19,1,22,13,28,28,28,13,13,24,1},
     {29,24,18,24,26,17,28,23,28,0,9,24,16,14,23,4},
     {14,21,3,4,3,24,9,14,12,27,8,22,8,11,16,28},
     {7,10,22,18,26,9,12,1,19,31,11,24,2,26,13,19},
     {29,16,19,14,3,0,31,30,7,7,26,17,10,4,25,28},
     {10,8,24,20,23,1,5,16,20,9,21,9,3,27,23,11},
     {17,12,16,31,19,26,18,4,17,18,9,8,4,0,14,2}};
    
    private static final int[] scores =
    {5,9,15,14,23,12,13,17,9,16};
    
    private static final int[][] evaluates =
    {{1,1,1,1,1,1,1,0,1,1,1,0,0,0,0,0},
     {1,1,1,0,1,1,1,1,1,0,1,0,0,0,0,0},
     {1,1,1,0,1,1,1,0,1,1,1,1,1,1,1,1},
     {1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1},
     {1,1,1,0,1,1,1,1,1,1,1,0,0,0,0,0},
     {1,1,1,0,1,1,1,1,1,1,1,0,1,0,1,1},
     {1,1,1,1,1,1,1,1,1,1,1,0,1,0,1,1},
     {1,1,1,1,1,1,1,1,1,0,1,1,1,0,1,1},
     {1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1},
     {1,1,1,1,1,1,0,0,1,1,1,0,1,0,1,0}};
    
    private TesterGame game;
    
    public HW6Comptest(TesterGame game, String name, double fpoints, double epoints, int timelimit) 
    {    
        super(name, fpoints, epoints, timelimit);
        this.game = game;
    }
    
    public HW6Comptest() 
    { 
        super(); 
    }
    
    public static Test suite()
    {
        TestSuite suite = new TestSuite();
        TesterGame[] games = new TesterGame[GAMES];
        
        for(int i=0; i<GAMES; i++){
            games[i] = new TesterGame(leaves[i], evaluates[i], scores[i]);
        }
        
        for(int i=0; i<GAMES; i++){
            TesterGame game = games[i];
            suite.addTest(new HW6Comptest(game, "NegaMax test", 2.0, 2.0, 30) { 
                public void runTest() { testNegaMax2(); } 
            });
        }
        
        for(int i=0; i<GAMES; i++){
            TesterGame game = games[i];
            suite.addTest(new HW6Comptest(game, "AlphaBeta test", 2.0, 2.0, 30) { 
                public void runTest() { testAlphaBeta(); } 
            });
        }
        
        for(int i=0; i<GAMES; i++){
            TesterGame game = games[i];
            suite.addTest(new HW6Comptest(game, "Pruning test", 2.0, 2.0, 30) { 
                public void runTest() { testPruning(); } 
            });
        }

        return suite; 
    } 

    public void testNegaMax2() 
    {
        game.reset();
        Evaluator eval = new TesterEvaluator();
        AmazonsPlayerNegaMax NM = new AmazonsPlayerNegaMax();
//        CompiledPlayerNegaMax NM = new CompiledPlayerNegaMax();

        double value = NM.negaMax(false, DEPTH, game, eval, null);
        Assert.assertEquals("Fails negaMax on game tree " + TesterGame.string(game.leaves), (int)game.score, (int)value);
    }

    public void testAlphaBeta() 
    {
        game.reset();
        Evaluator eval = new TesterEvaluator();
        AmazonsPlayerAlphaBeta AB = new AmazonsPlayerAlphaBeta();
//        CompiledPlayerAlphaBeta AB = new CompiledPlayerAlphaBeta();
        
        double value = AB.alphaBeta(false, DEPTH, game, -1000, 1000, eval, null);
        Assert.assertEquals("Fails alphaBeta on game tree " + TesterGame.string(game.leaves), (int)game.score, (int)value);
    }
    
    public void testPruning() 
    {
        game.reset();
        Evaluator eval = new TesterEvaluator();
        AmazonsPlayerAlphaBeta AB = new AmazonsPlayerAlphaBeta();
//        CompiledPlayerAlphaBeta AB = new CompiledPlayerAlphaBeta();

        double value = AB.alphaBeta(false, DEPTH, game, -1000, 1000, eval, null);
        int[] evaluates = game.check(DEPTH);
        Assert.assertTrue("Fails alphaBeta pruning on game tree " + TesterGame.string(game.leaves) + " expected: " + TesterGame.string(game.evaluates) + " but was: " + TesterGame.string(evaluates), Arrays.equals(evaluates, game.evaluates));
    }
}

/** 
 *  An interface for a Trie
 *
 *  You should not modify or handin this file.
 *
 *  You should read the comments very careful for specifics on how you should
 *  implement these functions.
 *
 *  @author Colin Rothwell
 */
public interface TrieInterface
{
    /**
     *  Insert the provided 3-gram into the trie.
     *
     *  Error handling: If the provided string does not contain exactly three
     *  words then this method should return -1 (and not modify the trie).
     */
    public int insertGram(String gram3);

    /**
     *  Return the most frequent word that follows the provided 2-gram.
     *
     *  Error handling: If the provided string does not contain exactly two
     *  words then this method should return null.
     */
    public String generateNext(String gram2);

    /**
     *  Returns the size (number of nodes) in the first level of the trie.
     */
    public int sizeLevel0();

    /** 
     *  Returns the size (number of nodes) in the middle level of the trie.
     */
    public int sizeLevel1();

    /**
     *  Returns the size (number of nodes) in the last level of the trie.
     */
    public int sizeLevel2();
}
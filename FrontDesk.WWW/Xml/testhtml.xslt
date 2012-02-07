<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="fd:TestSuite" xmlns:fd="urn:frontdesk-result">

<xsl:if test="count(fd:Test) > 0"> 
<font style="font-size: 8pt">Tests:</font><br />
</xsl:if>

<TABLE STYLE="font-size: 8pt;" cellspacing="0">
<TR>
<TD valign="top">

<pre><xsl:value-of select="fd:Error" /></pre>

<xsl:for-each select="fd:Test">
<TABLE STYLE="font-size: 8pt;" WIDTH="240">
<TR><TD><img src="attributes/cyl.gif" Align="AbsMiddle" /> Name: <b><xsl:value-of select="fd:Name" /></b><br />
Points: <xsl:value-of select="fd:Points" />  Time: <xsl:value-of select="fd:Time" />s
</TD></TR>		        
</TABLE>
</xsl:for-each>

</TD>
</TR>  
</TABLE>
</xsl:template>

</xsl:stylesheet>

  
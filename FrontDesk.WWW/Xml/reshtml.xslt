<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="fd:Result" xmlns:fd="urn:frontdesk-result">
<xsl:if test="fd:Success = 'flawless'">
<img src="attributes/good.gif" Align="AbsMiddle" />
 <font style="FONT-SIZE: 12pt" ><b>Evaluation successful!</b></font>
</xsl:if>
<xsl:if test="fd:Success = 'flawed'">
<img src="attributes/error.gif" Align="AbsMiddle" />
 <font style="FONT-SIZE: 12pt"><b>There are errors for this evaluation.</b></font>
</xsl:if>
<xsl:if test="fd:Success = 'criticallyflawed'">
<img src="attributes/error.gif" Align="AbsMiddle" />
 <font style="FONT-SIZE: 12pt" ><b>There are errors for this evaluation.</b></font>
</xsl:if>
<xsl:if test="fd:Success = 'depfail'">
<img src="attributes/warning.gif" Align="AbsMiddle" />
 <font style="FONT-SIZE: 12pt" ><b>Dependency failed. Test not run.</b></font>
</xsl:if>
<br /> <br />

<TABLE STYLE="font-size: 8pt;" CELLPADDING="5">
<TR>
<TD valign="top">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Failures</font><br /><br />
<xsl:for-each select="fd:Failure">
<TABLE STYLE="font-size: 8pt;" WIDTH="500"  
				          CELLPADDING="1">
<TR><TD><img src="attributes/error.gif" Align="AbsMiddle" /> 
<xsl:value-of select="fd:Name" /><br />
Points: <font color="#ff0000"><b>-<xsl:value-of select="fd:Points" /></b></font>
<br /><br />
<i><xsl:value-of select="fd:Message" /></i>
</TD></TR>		        
</TABLE><br />
</xsl:for-each>
</TD>

</TR>

<TR>
<TD valign="top">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Errors</font><br /><br />
<xsl:for-each select="fd:Error">
<TABLE STYLE="font-size: 8pt;" WIDTH="500"  
				          CELLPADDING="1">
<TR><TD><img src="attributes/nosub.gif" Align="AbsMiddle" /> <xsl:value-of select="fd:Name" /><br />
Points: <font color="#ff0000"><b>-<xsl:value-of select="fd:Points" /></b></font>
<br /><br />
<pre><xsl:value-of select="fd:Message" /></pre>
</TD></TR>		        
</TABLE><br />
</xsl:for-each>
</TD>
</TR>
<TR>
<TD valign="top">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Attributes</font><br /><br />
<TABLE STYLE="font-size: 8pt; border-collapse:collapse;" WIDTH="220" BORDERCOLOR="black" BORDER="1" RULES="all" CELLPADDING="3">
<TR bgcolor="#4768A3" STYLE="font-weight: bold; color: #ffffff;"><TD>Name</TD><TD>Value</TD></TR>
<TR><TD>Time</TD><TD><xsl:value-of select="fd:Time" />s</TD></TR>
<TR><TD>Test Count</TD><TD><xsl:value-of select="fd:Count" /></TD></TR>
<TR><TD>Test Toolkit</TD><TD><xsl:value-of select="fd:API" /></TD></TR>					        
</TABLE>
</TD>

</TR><TR>
<TD valign="top">
<font style="FONT-WEIGHT: bold; FONT-SIZE: 12pt">Messages</font><br /><br />
<DIV STYLE="OVERFLOW: auto; Width: 400px; Height: 200px;">
<pre><xsl:value-of select="fd:Msg" /></pre></DIV>
</TD>
</TR>

    
</TABLE>
</xsl:template>

</xsl:stylesheet>

  
<?xml version="1.0" encoding="UTF-8" ?>

<!DOCTYPE stylesheet [
	<!ENTITY tab "<xsl:text>&#9;</xsl:text>">
	<!ENTITY cr "<xsl:text>
</xsl:text>">
]>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
    <xsl:apply-templates/>
</xsl:template>

<xsl:template match="checkstyle">
	<ResultSubj xmlns="urn:frontdesk-result">&cr;
	<xsl:for-each select="file">
		<xsl:for-each select="error">
			&tab;<Comment>&cr;
			&tab;&tab;<Type>Warning</Type>&cr;
			&tab;&tab;<Points>0</Points>&cr;
			&tab;&tab;<Message><xsl:value-of select="@message" /></Message>&cr;
			&tab;&tab;<File><xsl:value-of select="../@name" /></File>&cr;
			&tab;&tab;<Line><xsl:value-of select="@line" /></Line>&cr;
			&tab;</Comment>&cr;
		</xsl:for-each>
	</xsl:for-each>
	</ResultSubj>
</xsl:template>

</xsl:stylesheet>
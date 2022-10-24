<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <div>
        <xsl:for-each select="Sites/Site">
          <a class="Links">
            <xsl:attribute name="href">/Default.aspx?q=<xsl:value-of select="@Name" /></xsl:attribute>
            <xsl:if test="string-length(@Name) &gt; 25">
              <xsl:value-of select="substring(@Name, 1, 25)" />...
            </xsl:if>
            <xsl:if test="string-length(@Name) &lt; 26">
              <xsl:value-of select="@Name" />
            </xsl:if>
          </a>
          <br></br>
        </xsl:for-each>
    </div>
  </xsl:template>
</xsl:stylesheet>
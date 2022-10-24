<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <div>
        <xsl:for-each select="Links/Link">
          <a class="Links">
            <xsl:attribute name="href">
              <xsl:value-of select="@Href" />
            </xsl:attribute>

            <xsl:if test="string-length(@Title) &gt; 25">
              <xsl:value-of select="substring(@Title, 1, 25)" />...
            </xsl:if>
            <xsl:if test="string-length(@Title) &lt; 26">
              <xsl:value-of select="@Title" />
            </xsl:if>
          </a>
          <br></br>
        </xsl:for-each>
    </div>
  </xsl:template>
</xsl:stylesheet>
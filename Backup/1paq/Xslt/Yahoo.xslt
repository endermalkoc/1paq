<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <ul>
        <xsl:for-each select="Links/Link">
          <li>
              <a>
                <xsl:attribute name="href">
                  <xsl:value-of select="@Href" />
                </xsl:attribute>
                <xsl:value-of select="@Title" />
              </a>             
          </li>
        </xsl:for-each>
    </ul>
  </xsl:template>
</xsl:stylesheet>
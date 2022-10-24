<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <table>
            <xsl:for-each select="Books/Book">
              <xsl:if test="position() &lt; 6">
                <tr>  
                  <td valign="top">
                    <xsl:if test="@ThumbnailUrl">
                    <a class="thumbnail">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@DetailPageUrl" />
                      </xsl:attribute>
                      
                      <img class="thumbnail">
                        <xsl:attribute name="src">
                          <xsl:value-of select="@ThumbnailUrl" />
                        </xsl:attribute>
                        <xsl:attribute name="title">
                          <xsl:value-of select="@Title" />
                        </xsl:attribute>
                        <xsl:attribute name="alt">
                          <xsl:value-of select="@Title" />
                        </xsl:attribute>
                        <xsl:attribute name="width">
                          <xsl:value-of select="@ThumbnailWidth" />
                        </xsl:attribute>
                        <xsl:attribute name="height">
                          <xsl:value-of select="@ThumbnailHeight" />
                        </xsl:attribute>
                      </img>
                    </a>
                  </xsl:if>
                  </td>
                  <td valign="top">
                    <a class="title">
                      <xsl:attribute name="title">
                        <xsl:value-of select="@Title" />
                      </xsl:attribute>
                      <xsl:attribute name="href">
                        <xsl:value-of select="@DetailPageUrl" />
                      </xsl:attribute>
                      <xsl:if test="string-length(@Title) &gt; 35">
                        <xsl:value-of select="substring(@Title, 1, 35)" />...
                      </xsl:if>
                      <xsl:if test="string-length(@Title) &lt; 36">
                        <xsl:value-of select="@Title" />
                      </xsl:if>
                    </a>
                    <br />
                    <xsl:if test="@Price">
                    Price : <xsl:value-of select="@Price" />
                    </xsl:if>
                  </td>
                </tr>
              </xsl:if>
            </xsl:for-each>
      
    </table>
  </xsl:template>
</xsl:stylesheet>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <table>
      <tr>
        <xsl:for-each select="Images/Image">
          <xsl:if test="position() &lt; 7">
            <xsl:call-template name="Image" />
          </xsl:if>
        </xsl:for-each>
      </tr>
      <tr>
        <xsl:for-each select="Images/Image">
          <xsl:if test="((position() &gt; 6) and (position() &lt; 13))">
            <xsl:call-template name="Image" />
          </xsl:if>
        </xsl:for-each>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="Image">
    <td valign="top">
      <a class="thumbnail">
        <xsl:attribute name="href">
          <xsl:value-of select="@Url" />
        </xsl:attribute>
        <img class="thumbnail">
          <xsl:attribute name="src">
            <xsl:value-of select="@ThumbnailUrl" />
          </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="@Description" />
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="@Description" />
          </xsl:attribute>
          <xsl:attribute name="width">
            <xsl:value-of select="@ThumbnailWidth" />
          </xsl:attribute>
          <xsl:attribute name="height">
            <xsl:value-of select="@ThumbnailHeight" />
          </xsl:attribute>
        </img>
      </a>
      <br />
      <a class="title">
        <xsl:attribute name="title">
          <xsl:value-of select="@Title" />
        </xsl:attribute>
        <xsl:attribute name="href">
          <xsl:value-of select="@Url" />
        </xsl:attribute>
        <xsl:if test="string-length(@Title) &gt; 35">
          <xsl:value-of select="substring(@Title, 1, 35)" />...
        </xsl:if>
        <xsl:if test="string-length(@Title) &lt; 36">
          <xsl:value-of select="@Title" />
        </xsl:if>
      </a>
    </td>
  </xsl:template>
</xsl:stylesheet>
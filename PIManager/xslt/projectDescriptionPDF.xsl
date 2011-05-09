<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:fo="http://www.w3.org/1999/XSL/Format"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:fn="http://www.w3.org/2005/xpath-functions">

  <xsl:param name="level1" select="string('&#8226;')" />
  <xsl:param name="level2" select="string('-')" />
  <xsl:param name="level3" select="string('>')" />

  <xsl:template match="/">
    <fo:root>
      <fo:layout-master-set>
        <fo:simple-page-master master-name="pageA4" page-height="297mm"
					page-width="210mm" margin-top="15mm" margin-bottom="15mm"
					margin-left="25mm" margin-right="20mm">

          <fo:region-body margin-bottom="20mm" />
          <fo:region-after extent="10mm" />

        </fo:simple-page-master>
      </fo:layout-master-set>

      <fo:page-sequence master-reference="pageA4">
        <fo:static-content flow-name="xsl-region-after">
          <fo:block text-align="center" font-size="10pt">
            Page <fo:page-number />
          </fo:block>
        </fo:static-content>
        <fo:flow flow-name="xsl-region-body">
          <xsl:apply-templates />
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>

  <xsl:template match="project">
    <fo:block font-size="12pt" font-weight="bold">
      Titre: <xsl:value-of select="title" />
    </fo:block>
    <fo:block font-size="12pt" font-weight="bold">
      Abréviation: <xsl:value-of select="abreviation" />
    </fo:block>
    <fo:block>
      <xsl:apply-templates select="description" />
    </fo:block>
  </xsl:template>

  <xsl:template match="description">
    <fo:block font-size="12pt" font-weight="bold">
      Description:
    </fo:block>
    <fo:block>
      <xsl:value-of select="paragraph" />
    </fo:block>
    <fo:list-block provisional-distance-between-starts="18pt" provisional-label-separation="3pt">
      <xsl:call-template name="recurselist">
        <xsl:with-param name="node" select="list" />
      </xsl:call-template>
    </fo:list-block>
  </xsl:template>

  <xsl:template name="recurselist">
    <xsl:param name="node" />
    <xsl:param name="level" />

    <xsl:for-each select="$node/*">
      <xsl:if test="name(current())='listItem'">
        <xsl:if test="count(current()/*)=2">
          <fo:list-item>
            <fo:list-item-label end-indent="label-end()">
              <fo:block>&#8226;</fo:block>
            </fo:list-item-label>
            <fo:list-item-body start-indent="body-start()">
              <fo:block>
                <xsl:value-of select="current()/text" />
                <fo:list-block provisional-distance-between-starts="18pt"
										provisional-label-separation="3pt">
                  <xsl:call-template name="recurselist">
                    <xsl:with-param name="node" select="." />
                  </xsl:call-template>
                </fo:list-block>
              </fo:block>
            </fo:list-item-body>
          </fo:list-item>
        </xsl:if>
        <xsl:if test="count(current()/*)=1">
          <fo:list-item>
            <fo:list-item-label end-indent="label-end()">
              <fo:block>&#8226;</fo:block>
            </fo:list-item-label>
            <fo:list-item-body start-indent="body-start()">
              <fo:block>
                <xsl:value-of select="current()/text" />
              </fo:block>
            </fo:list-item-body>
          </fo:list-item>
        </xsl:if>
      </xsl:if>
      <xsl:if test="name(current())!='listItem'">
        <xsl:call-template name="recurselist">
          <xsl:with-param name="node" select="." />
        </xsl:call-template>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>

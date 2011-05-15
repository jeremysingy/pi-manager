<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:fo="http://www.w3.org/1999/XSL/Format"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:fn="http://www.w3.org/2005/xpath-functions">


  <xsl:param name="IMAGE">data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHMAAABkCAIAAAA68piSAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAK/xJREFUeNrsfWlsZFd23r1vf7XvxSJZ3JvqVd1qdavVGkmjkce7J84giQPDf2LAMfI7iZM/BvwjRvInQIAkhuHEMRI4iTNBPPEY42C826PR1pJ6Z7O5VJEskrWQtW9vfzfn3FekWvEKBAY0wy6p2cVX771679xzv/N955z7mjLGyPPX38BLeG6C55Z9btnnr+eWfW7Z55Z9/npu2eeWfW7Z568/85IYsSnaVyBM8F1nf3dze2stnc5cvnZbUhXCfMLYztbm7k4pl82tXLikaiFCCWx8urbWqB+GQ6GXbt0SpZBjmyJluxtPyjs7uVz+ys1bVJBgN+Y5le3Nra3NpeXl+aUVUYXDxTNh2U/dlhLLHFcq+9VqtTcYK3rq4ovXYKPR7+5V...9y69z8z3/1S1ez6tl4lCeSdkPCZDOS/cHA+Np7u1vj6B+X/P/yoBH0kxzVR9+65909zjys7T7eKGFyVFb6zP7NP97dbqd/p0L/10ZJ9oji0qFj/fq9J49t9bcO29+495CvcBb6nf47m539sf71j9v3SlUCTFk4E/+6CrZ6QdixbFRHiqjOz07DLE5Elcv5CN/BLma82HyfJCzC5pPZGSJ6gu9kqeLplhompt16JRcTJNOVPcGSpmiRCImQEl3OxPlSYDHhK7MKJZaUCYeSvEAvnQ2npQPGIrge3xD8ni/G7w/pbz/YuZpIf2UpIciWLYU16j/q0N++s3F1JvX9F6aYO8T6gGQ/7UR/6/7e5UL6R1dtV0jIZCiZWqOpfX13lFPZV69FsPNTlSF4PeiMvvG4emU2/UMLqRDg7dl45CR9/m80/01Xa56/nlv2uWWfW/b567lln1v2+eu5ZZ9b9rlln79OX/9XgAEAmcoW866WFVQAAAAASUVORK5CYII=</xsl:param>

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
          <fo:block>
			  <fo:external-graphic src="{$IMAGE}" />
          </fo:block>
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

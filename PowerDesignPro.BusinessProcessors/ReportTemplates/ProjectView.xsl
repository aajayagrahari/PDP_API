<?xml version="1.0" encoding="UTF-8" ?>
<!-- updated for use with Apsose.PDf -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:vb="http://generac.com/scripting">
  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" />
  <xsl:param name="HarmonicAnalysisGraph1"></xsl:param>
  <xsl:param name="HarmonicAnalysisGraph2"></xsl:param>
  <msxsl:script implements-prefix="vb" language="VB">
    <![CDATA[ 
		'
		' Returns a date in specified format
		'
		Function FormatDateTime(ByVal date1 As String, ByVal format As String) As String
		    If date1.Length = 0 Then Return New String(" ", format.Length)
		    Dim d1 As Date = Date.Parse(date1)
		    Return d1.ToString(format)
		End Function	
    
		Function CurrentDate() As String
		    Dim d1 as String = FormatDateTime(Now, "MM/dd/yyyy")
		    Return d1
		End Function	
    
		
		'
		' Checks if str2 is the same as str1, after trimming any trailing spaces and commas
		'
	    Function IsDuplicateDesc(ByVal str1 As String, ByVal str2 As String) As Boolean
            str1 = str1.TrimEnd(Nothing)
            str1 = str1.TrimEnd(",")
            str2 = str2.TrimEnd(Nothing)
            str2 = str2.TrimEnd(",")
            Return str2 = str1
        End Function
        
		'
		' Checks if str2 is the same as str1, after trimming any trailing spaces and commas
		'
	    Function ReplaceDesc(ByVal assignValue As String, ByVal formatDesc As String) As string
        If formatDesc.Length > 0 AndAlso assignValue.Length > 0 Then
          Dim tempDecimal As Double
          If Double.TryParse(assignValue, tempDecimal) Then
              Return String.Format(formatDesc, tempDecimal)
          Else
              Return String.Format(formatDesc, assignValue)
          End If

        Else
          Return assignValue
        End If     
      End Function        
	]]>
  </msxsl:script>
  <xsl:template match="/">
    <Pdf xmlns="Aspose.Pdf" Title="Project Details"  Author="Generac Power Systems, Inc." FontName="Courier New" DefaultFontName="Courier New" FontSize="9" >

      <Section PageSize="Letter" PageMarginBottom=".5inch" PageMarginLeft=".5inch" PageMarginRight=".5inch" PageMarginTop=".5inch" IsLandscape ="true">

        <Header DistanceFromEdge=".75inch" MarginBottom=".15inch">
          <Table ColumnWidths="40% 30% 20% 10%"  >
            <Border>
              <Bottom LineWidth=".5"/>
              <Top BorderStyle="None"/>
              <Right BorderStyle="None"/>
              <Left BorderStyle="None"/>
            </Border>            
            <Row>
              <Cell RowSpan="2">
                <Image File="http://genconnect.generac.com/media/images/generaclogo.jpg" />
                <Text>
                  <Segment FontName="Arial" FontSize="8">
                    Hwy 59 &amp; Hillside Rd. Waukesha, WI 53187 USA#$NL (262) 544-4811 fax: (262) 698-9372 www.generac.com
                  </Segment>
                </Text>
              </Cell>
              <Cell></Cell>
              <Cell></Cell>
              <Cell></Cell>
            </Row>
            <Row>
              <Cell></Cell>
              <Cell></Cell>
              <Cell Alignment="Right" VerticalAlignment="Bottom">
                <Text>
                  <Segment>
                   Page $p of $P
                  </Segment>                  
                  <Segment>
                    #$NL
                  </Segment>
                  <Segment>
                    <xsl:value-of select="vb:CurrentDate()" />
                    #$NL#$NL
                  </Segment>
                </Text>
              </Cell>
            </Row>
          </Table>
        </Header>
        <Text>
          <Segment FontName="Arial" FontSize="14" IsTrueTypeFontBold="true">
            <xsl:value-of select="//PDPView/Project/ProjectSummaryLabel" />
          </Segment>
        </Text>     
        <Text>
          <Segment>#$NL</Segment>
        </Text>  
        <!--Project Summary-->
        <Table ColumnWidths="50% 50%">       
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/ContactInformationLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/ProjectNameLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/ProjectName" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/NameLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Name" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/SpecRefLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/SpecRef" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/DescriptionLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Description" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/ContactLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/Contact" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/EmailLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/Email" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/PreparedByLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/PreparedByNameLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/PreparedByName" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/PreparedByCompanyLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/PreparedByCompany" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/PreparedByPhoneLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/PreparedByPhone" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Project/PreparedByEmailLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Project/PreparedByEmail" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>              
            </Cell>
          </Row>
        </Table> 
        <Text>
          <Segment>#$NL</Segment>
        </Text>         
       <!--solution-->  
        <!--<Text>
          <Segment>#$NL</Segment>
        </Text>        
        <Table ColumnWidths="100%">       
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <Table ColumnWidths="30% 70%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
              </Table>
            </Cell>
          </Row>
        </Table>-->     
        <Table ColumnWidths="50% 50%">       
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <!--Environment-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Environment/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Environment/AmbientTempLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Environment/AmbientTemp" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Environment/ElevationLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Environment/Elevation" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>

              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Electrical Configuration-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/PhaseLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/Phase" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/FrequencyLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/Frequency" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/VoltageNorminalLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/VoltageNorminal" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/VoltageSpecificLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/ElectricalConf/VoltageSpecific" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                      
              </Table>

              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Maximum Allowable Transients-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/MaximumRunningLoadLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/MaximumRunningLoad" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/VoltageDipLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/VoltageDip" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/FrequencyDipLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableTransients/FrequencyDip" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>              
              </Table>

              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Load Sequence Configuration-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/LoadSeqConf/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/LoadSeqConf/Cyclic1Label" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/LoadSeqConf/Cyclic1Label" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/LoadSeqConf/Cyclic2Label" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/LoadSeqConf/Cyclic2" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
            <Cell>
              <!--Units-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Units/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Units/UnitsLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Units/UnitsValue" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Engine-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Engine/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Engine/DutyLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Engine/Duty" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/Engine/FuelLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/Engine/Fuel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>             
              </Table>
              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Regulatory Information-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/RegulatoryInformation/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/RegulatoryInformation/RegulatoryFiltersLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/RegulatoryInformation/RegulatoryFilters" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/RegulatoryInformation/ApplicationLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/RegulatoryInformation/Application" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>             
              </Table>
              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Generator Configuration-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/SoundLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/Sound" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/FuelTankLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/FuelTank" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/RunTimeLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/GeneratorConfiguration/RunTime" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>              
              </Table>
              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Max Allowable Voltage Distortion (% THVD)-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableVoltageDistortion/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableVoltageDistortion/ContinuousLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableVoltageDistortion/Continuous" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableVoltageDistortion/MomentaryLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/Solution/MaxAllowableVoltageDistortion/Momentary" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>    
             
            </Cell>
          </Row>
        </Table>
        <Text>
          <Segment>
            <!--Page Breaker-->
            #$NP
          </Segment>
        </Text>
        <!--Generator and Load Summary-->
        <Text>
          <Segment FontName="Arial" FontSize="14" IsTrueTypeFontBold="true">
            <xsl:value-of select="//PDPView/GensetLoadSummary/TitleLabel" />
          </Segment>
        </Text>
        <Text>
          <Segment>#$NL</Segment>
        </Text>
        <Table ColumnWidths="50% 50%">
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <!--Selected generator & Alternator-->
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/ProductFamilyMethodLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/ProductFamilyMethod" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/ProductFamilyLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/ProductFamily" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>                
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/ModuleSizeLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/ModuleSize" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>   
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/SizingMethodLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/SizingMethod" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/GeneratorLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/Generator" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>  
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/AlternatorLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/SelectedGeneratorAlternator/Alternator" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>               
              </Table>
              
              <Text height="4">
                <Segment></Segment>
              </Text>
              
              <!--Load Summary-->
              <Table ColumnWidths="17% 16% 17% 16% 17% 17% " DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="6" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <!--<DefaultCellBorder>
                    <Bottom LineWidth="1" Color="rgb 0 0 0"/>
                    <Top BorderStyle="None"/>
                    <Right BorderStyle="None"/>
                    <Left BorderStyle="None"/>
                  </DefaultCellBorder>-->                  
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TransientsLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicsLabel" />
                      </Segment>
                    </Text>
                  </Cell>                
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningKWLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningKW" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TransientsLkWStepLabel" />:
                      </Segment>
                    </Text>
                  </Cell> 
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TransientsLkWStep" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicskVALabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicskVA" />
                      </Segment>
                    </Text>
                  </Cell>                                
                </Row>   
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningkVALabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningkVA" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TransientsLkWPeak" />:
                      </Segment>
                    </Text>
                  </Cell> 
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TransientsLkWPeakLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicsTHIDContLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicsTHIDCont" />
                      </Segment>
                    </Text>
                  </Cell>                                
                </Row>   
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningPFLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/RunningPF" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/TransientsLkVAStepLabel" />:
                      </Segment>
                    </Text>
                  </Cell> 
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSuammary/TransientsLkVAStep" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicsTHIDPeakLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadSummary/HarmonicsTHIDPeak" />
                      </Segment>
                    </Text>
                  </Cell>                                
                </Row>              
              </Table>            
            </Cell>
            <Cell>
              <!--genset and alternator-->
              <Table ColumnWidths="17% 16% 17% 16% 17% 17% " DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="6" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TitleLabel1" />
                        #$NL
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TitleLabel2" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <!--<DefaultCellBorder>
                    <Bottom LineWidth="1" Color="rgb 0 0 0"/>
                    <Top BorderStyle="None"/>
                    <Right BorderStyle="None"/>
                    <Left BorderStyle="None"/>
                  </DefaultCellBorder>-->
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LoadLevelLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TransientsLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/HarmonicsLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LoadLevelRunningLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LoadLevelRunning" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TransientsFdipLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TransientsFdip" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/HarmonicsTHVDContLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/HarmonicsTHVDCont" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LoadLevelPeakLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LoadLevelPeak" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TransientsVdipLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/TransientsVdip" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/HarmonicsTHVDPeakLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/HarmonicsTHVDPeak" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitProjectLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitFdipLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitFdip" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitTHVDContLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitTHVDCont" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitMaxLoadingLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitMaxLoading" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitVdipLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitVdip" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitTHVDPeakLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="6">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/GensetAlternator/LimitTHVDPeak" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
          </Row>
          <Row>
            <Cell ColumnsSpan="2">
              <Text height="4">
                <Segment></Segment>
              </Text>              
            </Cell>
          </Row>
          <Row>
            <Cell ColumnsSpan="2">
              <!--Load List-->
              <Table ColumnWidths="8% 20% 8% 8% 8% 8% 8% 8% 8% 8% 8%" DefaultCellPaddingBottom="2" DefaultCellPaddingRight="2" DefaultCellPaddingTop="2" DefaultCellPaddingLeft="2" MarginLeft="10" MarginRight="0">
                <DefaultCellBorder>
                  <All LineWidth="0.1"></All>
                </DefaultCellBorder>
                <Border>
                  <All LineWidth="1"></All>
                </Border>
                <!--Header-->
                <Row BackgroundColor="LightGray">
                  <Cell ColumnsSpan="2" Alignment="Left">
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/StartingHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/RunningHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="3" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/HarmonicCurrentDistortionHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/LimitsHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row BackgroundColor="LightGray">
                  <Cell Alignment="Left">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/SequenceHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Left">                   
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/DescriptionHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/StartingkWHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/StartingkVAHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/RunningHeaderkWColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/RunningHeaderkVAColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/HarmonicCurrentDistortionPeakHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/HarmonicCurrentDistortionContHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/HarmonicCurrentDistortionkVAHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/LimitsVdipHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="//PDPView/GensetLoadSummary/LoadList/LimitsFdipHeaderColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <xsl:for-each select="//PDPView/GensetLoadSummary/LoadList/LoadItemList/LoadItem">
                  <Row>
                  <xsl:if test="isSummaryRow=1">
                    <xsl:attribute name="BackgroundColor">
                      <xsl:text>rgb 220 220 220</xsl:text>
                    </xsl:attribute>
                  </xsl:if>

                  <Cell Alignment="Left">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="concat(GroupStepLabel,' ',Sequence)" />
                        
                        <xsl:if test="isSummaryRow=1">
                          #$NL
                          <xsl:value-of select="/PDPView/GensetLoadSummary/LoadList/SummaryLabel"/>
                        </xsl:if>
                        
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Left">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:if test="isSummaryRow=0">
                          <xsl:value-of select="ItemDesc" />
                          #$NL
                          <xsl:value-of select="concat(ItemKVA,', ',ItemPF)"/>
                          #$NL
                          <xsl:value-of select="ItemHarmonics" />                        
                        </xsl:if>
                        <xsl:if test="isSummaryRow=1">
                          <xsl:value-of select="/PDPView/GensetLoadSummary/LoadList/SummaryAllLoadsLabel"/>
                          #$NL
                          <xsl:value-of select="concat(SequencePeak,' ',/PDPView/GensetLoadSummary/LoadList/SummaryAllLoadsLabel)"/>
                          #$NL
                          <xsl:value-of select="concat(ApplicationPeak,' ',/PDPView/GensetLoadSummary/LoadList/ApplicationPeakLabel)"/>
                        </xsl:if>
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="StartingkW" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="StartingkVA" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="RunningkW" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="RunningkVA" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="HoarmonicCurrentDistortionPeak" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="HoarmonicCurrentDistortionCont" /> %
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:value-of select="HoarmonicCurrentDistortionkVA" />
                      </Segment>
                    </Text>
                  </Cell>

                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:if test="isSummaryRow=0">
                          <xsl:value-of select="LimitsVdip" /> %
                        </xsl:if>
                        <xsl:if test="isSummaryRow=1">
                          <xsl:value-of select="SequencePeakLimitVdip" /> %
                          #$NL
                          <xsl:value-of select="ApplicationPeakLimitVdip" /> volts
                        </xsl:if>                        
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7">
                        <xsl:if test="isSummaryRow=0">
                          <xsl:value-of select="LimitsFdip" /> hertz
                        </xsl:if>
                        <xsl:if test="isSummaryRow=1">
                          <xsl:value-of select="SequencePeakLimitFdip" /> %
                          #$NL
                          <xsl:value-of select="ApplicationPeakLimitFdip" /> hertz
                        </xsl:if>
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                </xsl:for-each>
              </Table>
            </Cell>
          </Row>
        </Table>

        <Text>
          <Segment>
            <!--Page Breaker-->
            #$NP
          </Segment>
        </Text>

        <!--Transient Analysis-->
        <Text>
          <Segment FontName="Arial" FontSize="14" IsTrueTypeFontBold="true">
            <xsl:value-of select="//PDPView/TransientAnalysis/TitleLabel" />
          </Segment>
        </Text>
        <Text>
          <Segment>#$NL</Segment>
        </Text>
        <Table ColumnWidths="50% 50%">
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/SequenceLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/Sequence" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/LoadLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/Load" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/StartingKVALabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/StartingKVA" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/VdipToleranceLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/VdipTolerance" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/VdipExpectedLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientReq/VdipExpected" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/SequenceLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/Sequence" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/LoadLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/Load" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/StartingKVALabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/StartingKVA" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/VdipToleranceLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/VdipTolerance" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/VdipExpectedLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EngineTransientReq/VdipExpected" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
          </Row>
          <Row>
            <Cell ColumnsSpan="2">
              <Text>
                <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                  #$NL
                </Segment>
              </Text>
            </Cell>
          </Row>
          <Row>
            <Cell Alignment="Center">
              <Text>
                <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                  <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/TitleLabel" />
                </Segment>
              </Text>
            </Cell>
            <Cell Alignment="Center">
              <Text>
                <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                  <xsl:value-of select="//PDPView/TransientAnalysis/EnginTransientAnalysis/TitleLabel" />
                </Segment>
              </Text>
            </Cell>
          </Row>
          <Row>
            <Cell Alignment="Center">
              <Table ColumnWidths="20% 20% 20% 20% 20%" DefaultCellPaddingBottom="2" DefaultCellPaddingRight="2" DefaultCellPaddingTop="2" DefaultCellPaddingLeft="2" MarginLeft="10" MarginRight="0">
                <DefaultCellBorder>
                  <All LineWidth="0.1"></All>
                </DefaultCellBorder>
                <Border>
                  <All LineWidth="1"></All>
                </Border>
                <Row BackgroundColor="LightGray">
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/SequenceColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/AllowableVdipColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/ExpectedVdipColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/SequenceStartingKVAColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/LargestTransientLoadColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <xsl:for-each select="//PDPView/TransientAnalysis/AlternatorTransientAnalysis/AnalysisItemList/AnalysisItem">
                  <Row>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="concat(GroupStepLabel,' ',Sequence)" />
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="AllowableVdip" /> %
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="ExpectedVdip" /> %
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="SequenceVdip" />
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="LargestTransientLoad" />
                        </Segment>
                      </Text>
                    </Cell>
                  </Row>
                  </xsl:for-each>
              </Table>
            </Cell>
            <Cell>
              <Table ColumnWidths="20% 20% 20% 20% 20%" DefaultCellPaddingBottom="2" DefaultCellPaddingRight="2" DefaultCellPaddingTop="2" DefaultCellPaddingLeft="2" MarginLeft="10" MarginRight="0">
                <DefaultCellBorder>
                  <All LineWidth="0.1"></All>
                </DefaultCellBorder>
                <Border>
                  <All LineWidth="1"></All>
                </Border>
                <Row BackgroundColor="LightGray">
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EnginTransientAnalysis/SequenceColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EnginTransientAnalysis/AllowableVdipColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EnginTransientAnalysis/ExpectedVdipColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EnginTransientAnalysis/SequenceStartingKVAColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/TransientAnalysis/EnginTransientAnalysis/LargestTransientLoadColumnLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <xsl:for-each select="//PDPView/TransientAnalysis/EnginTransientAnalysis/AnalysisItemList/AnalysisItem">
                  <Row>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="concat(GroupStepLabel,' ',Sequence)" />
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="AllowableVdip" /> %
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="ExpectedVdip" /> %
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="SequenceVdip" />
                        </Segment>
                      </Text>
                    </Cell>
                    <Cell Alignment="Center">
                      <Text>
                        <Segment FontName="Arial" FontSize="7">
                          <xsl:value-of select="LargestTransientLoad" />
                        </Segment>
                      </Text>
                    </Cell>
                  </Row>
                </xsl:for-each>
              </Table>
            </Cell>
          </Row>
          <Row>
            <Cell ColumnsSpan="2">
              <Text>
                <Segment FontName="Arial" FontSize="8">
                  #$NL
                  <xsl:value-of select="//PDPView/TransientAnalysis/Note" />
                </Segment>
              </Text>
            </Cell>
          </Row>
        </Table>
        <Text>
          <Segment>
            <!--Page Breaker-->
            #$NP
          </Segment>
        </Text>
        <!--Harmonic Analysis-->
        <!--Two Harmotic Analysis pages will be shown by using loop-->
        <xsl:for-each select="//PDPView/HarmonicAnalysisList/HarmonicAnalysis">
            <Text>
              <Segment FontName="Arial" FontSize="14" IsTrueTypeFontBold="true">
                <xsl:value-of select="TitleLabel" />
              </Segment>
            </Text>
            <Text>
              <Segment>#$NL</Segment>
            </Text>
            <Table ColumnWidths="50% 50%">
              <Border>
                <All BorderStyle="None"/>
              </Border>
              <Row>
                <Cell>
                  <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                            <xsl:value-of select="HarmonicProfileLabel" />:
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8">
                            <xsl:value-of select="HarmonicProfile" />
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                            <xsl:value-of select="kVANonlinearLoadLabel" />:
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8">
                            <xsl:value-of select="kVANonlinearLoad" />
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                            <xsl:value-of select="kVABaseLabel" />:
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8">
                            <xsl:value-of select="kVANBase" />
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                  </Table>
                </Cell>
                <Cell>
                  <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                            <xsl:value-of select="SequenceLabel" />:
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8">
                            <xsl:value-of select="Sequence" />
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                            <xsl:value-of select="THIDLabel" />:      <xsl:value-of select="THID" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8">
                            <xsl:value-of select="THVDLabel" />:      <xsl:value-of select="THVD" /> %
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                            <xsl:value-of select="SelectedSeqHarmonicAlternatorLoadingLabel" />:
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="8">
                            <xsl:value-of select="SelectedSeqHarmonicAlternatorLoading" /> %
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                  </Table>
                </Cell>
              </Row>
              <Row>
                <Cell ColumnsSpan="2" Alignment="Center">
                  <Text>
                    <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                      <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/TitleLabel" />
                    </Segment>
                  </Text>
                  <Table ColumnWidths="11% 9% 9% 9% 9% 9% 9% 9% 9% 9%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                    <DefaultCellBorder>
                      <All LineWidth="0.1"></All>
                    </DefaultCellBorder>
                    <Border>
                      <All LineWidth="1"></All>
                    </Border>
                    <!--header-->
                    <Row BackgroundColor="LightGray">
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/ProfileLabel" />
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            3rd
                          </Segment>
                        </Text>
                      </Cell> 
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            5th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            7th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            9th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            11th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            13th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            15th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            17th
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            19th
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/CurrentLabel" />
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current3" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current5" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current7" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current9" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current11" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current13" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current15" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current17" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current19" /> %
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                    <Row>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7" IsTrueTypeFontBold="true">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/VoltageLabel" />
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage3" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage5" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage7" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage9" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage11" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage13" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage15" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Voltage17" /> %
                          </Segment>
                        </Text>
                      </Cell>
                      <Cell>
                        <Text>
                          <Segment FontName="Arial" FontSize="7">
                            <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/Current19" /> %
                          </Segment>
                        </Text>
                      </Cell>
                    </Row>
                  </Table>
                </Cell>
              </Row>
              <Row>
                <Cell ColumnsSpan="2">
                  <Text height="4">
                    <Segment></Segment>
                  </Text>              
                </Cell>
              </Row>
              <Row>
                <Cell ColumnsSpan="2" Alignment="Center">

                  <!--<Image File="HarmonicAnalysis.PNG"/>-->
                  <Image File="HarmonicAnalysis.PNG">
                    <xsl:attribute name="File">
                      <xsl:value-of select="HarmonicCurrentAdnVoltageProfiles/GraphImagePath" /> 
                    </xsl:attribute>
                </Image>
                </Cell>
              </Row>
            </Table>
            
            <Text>
              <Segment>
                <!--Page Breaker-->
                #$NP
              </Segment>
            </Text>
        </xsl:for-each>
        
        <!--Gas Piping-->
        <Text>
          <Segment FontName="Arial" FontSize="14" IsTrueTypeFontBold="true">
            <xsl:value-of select="//PDPView/GasPiping/TitleLabel" />
          </Segment>
        </Text>
        <Text>
          <Segment>#$NL</Segment>
        </Text>
        <Table ColumnWidths="50% 50%">
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/SigingMethodLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/SigingMethod" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/PipeSizeLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/PipeSize" /> in
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/ProductFamilyLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/ProductFamily" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/GeneratorLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/Generator" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/FuelTypeLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/FuelType" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/FuelConsumptionLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/FuelConsumption" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/MinimumPressureLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/MinimumPressure" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell ColumnsSpan="2">
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GeneratorSummary/SummaryNote" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>                
              </Table>
            </Cell>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/PressureDropLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/PressureDrop" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/PercentageAllowableLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/PercentageAllowable" /> %
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/AvailablePressureLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingSolution/AvailablePressure" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
          </Row>
          <Row>
            <Cell ColumnsSpan="2">
              <Text height="4">
                <Segment></Segment>
              </Text>              
            </Cell>
          </Row>
          <Row>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/SupplyGasPressureLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/SupplyGasPressure" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/LenghOfRunLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/LenghOfRun" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/NumberOf90ElbowsLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/NumberOf90Elbows" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/Numberof45ElblowsLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/Numberof45Elblows" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/NumberOfTeesLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/GasPiping/GasPipingInput/NumberOfTees" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
            <Cell>
              
            </Cell>
          </Row>
        </Table>
        <Text height="4">
          <Segment></Segment>
        </Text>
        <Text>
          <Segment FontName="Arial" FontSize="8">
            <xsl:value-of select="//PDPView/GasPiping/Note" />
          </Segment>
        </Text>        
        <Text>
          <Segment>
            <!--Page Breaker-->
            #$NP
          </Segment>
        </Text>
        <!--Exhaust Piping-->
        <Text>
          <Segment FontName="Arial" FontSize="14" IsTrueTypeFontBold="true">
            <xsl:value-of select="//PDPView/ExhaustPiping/TitleLabel" />
          </Segment>
        </Text>
        <Text>
          <Segment>#$NL</Segment>
        </Text>
        <Table ColumnWidths="50% 50%">
          <Border>
            <All BorderStyle="None"/>
          </Border>
          <Row>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/SigingMethodLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/SigingMethod" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/PipeSizeLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/PipeSize" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/ProductFamilyLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/ProductFamily" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/GeneratorLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/Generator" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/TotalExahustFlowLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/TotalExahustFlow" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/MaxBackPressureLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustGeneratorSummary/MaxBackPressure" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustSolution/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustSolution/PressureDropLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhaustSolution/PressureDrop" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>
          </Row>
          <Row>
            <Cell ColumnsSpan="2">
              <Text height="4">
                <Segment></Segment>
              </Text>
            </Cell>
          </Row>
          <Row>
            <Cell>
              <Table ColumnWidths="50% 50%" DefaultCellPaddingBottom="4" DefaultCellPaddingRight="4" DefaultCellPaddingTop="4" DefaultCellPaddingLeft="4" MarginLeft="10" MarginRight="0">
                <Border>
                  <All LineWidth="1" Color="rgb 0 0 0"/>
                </Border>
                <Row>
                  <Cell ColumnsSpan="2" Alignment="Center">
                    <Text>
                      <Segment FontName="Arial" FontSize="10" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/TitleLabel" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/LenghOfRunLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/LenghOfRun" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/NumberOfStandardElbowsLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/NumberOfStandardElbows" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/NumberOfLongLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/NumberOfLong" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
                <Row>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8" IsTrueTypeFontBold="true">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/Numberof45ElblowsLabel" />:
                      </Segment>
                    </Text>
                  </Cell>
                  <Cell>
                    <Text>
                      <Segment FontName="Arial" FontSize="8">
                        <xsl:value-of select="//PDPView/ExhaustPiping/ExhausPipingInput/Numberof45Elblows" />
                      </Segment>
                    </Text>
                  </Cell>
                </Row>
              </Table>
            </Cell>            
            <Cell></Cell>
          </Row>
        </Table>
        <Text height="4">
          <Segment></Segment>
        </Text>
        <Text>
          <Segment FontName="Arial" FontSize="8">
            <xsl:value-of select="//PDPView/ExhaustPiping/Note" />
          </Segment>
        </Text>        
      </Section>
    </Pdf>
  </xsl:template>
</xsl:stylesheet>


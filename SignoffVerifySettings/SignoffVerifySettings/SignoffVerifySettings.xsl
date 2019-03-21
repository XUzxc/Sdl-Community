<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>
			<style>
				.title{
				font-family: Verdana;
				color: #6E7E82;
				font-size: 130%;
				}
				.filesTile{
				font-family: Verdana;
				color: #6E7E82;
				font-size: 100%;
				}
				
				#files{
				 font-family: Verdana, Helvetica, sans-serif;
				 border-collapse: collapse;
				 width: 100%;				 
				}
			  #files td {
				border: 1px solid #ddd;
				padding: 8px;
				font-size: 80%;
				}
				#files tr:nth-child(even){background-color: #f2f2f2;}
				#files tr:hover {background-color: #ddd;}
				
				#files th {
				  padding-top: 12px;
				  padding-bottom: 12px;
				  text-align: left;
				  background-color: #5e899e;
				  color: white;
					font-size: 85%;
				}
			
			</style>
			<head>
				<p class="title">Signoff Verify Settings</p>
			</head>
      <body>
				<b><xsl:text>Project: </xsl:text></b>	<xsl:value-of select="//ProjectInformation/Project/@Name"/>
				<br></br>
				
				<b><xsl:text>Source Language: </xsl:text>	</b> <xsl:value-of select="//ProjectInformation/SourceLanguage/@DisplayName"/>
				<br></br>
				
				<xsl:for-each select="//ProjectInformation/TargetLanguages/TargetLanguage">
					<b><xsl:text>Target Language: </xsl:text></b> <xsl:value-of select="DisplayName"/>
					<br></br>
				</xsl:for-each>
				
				<b><xsl:text>Run at (project level): </xsl:text></b>	<xsl:value-of select="//ProjectInformation/RunAt"/>
				<br></br>

				<xsl:for-each select="//ProjectInformation/TranslationMemories/TranslationMemory">
					<b><xsl:text>Translation Memory: </xsl:text></b> <xsl:value-of select="Name"/>
					<br></br>
				</xsl:for-each>

				<xsl:for-each select="//ProjectInformation/Termbases/Termbase">
					<b><xsl:text>Termbase: </xsl:text></b> <xsl:value-of select="Name"/>
					<br></br>
				</xsl:for-each>

				<b><xsl:text>RegEx rules: </xsl:text></b> <xsl:value-of select="//ProjectInformation/RegExRules"/>
				<br></br>
				<b><xsl:text>Check RegEx: </xsl:text></b> <xsl:value-of select="//ProjectInformation/CheckRegEx"/>
				<br></br>

				<b><xsl:text>QA Checker: </xsl:text></b>	<xsl:value-of select="//ProjectInformation/QAChecker"/>
				<br></br><br></br><br></br><br></br>
		
			 <b class="filesTile"><xsl:text>Language Files </xsl:text></b>
			 <br></br>
			 <br></br>				
				<div style="overflow-x:auto;">
					<table id="files">
						<tr>
							<th>Name</th>
							<th>Target language</th>
							<th>Run at</th>
							<th>Phase</th>
							<th>Is current assigned</th>
							<th>Assignees number</th>
							<th>Number Verifier</th>
						</tr>
						<xsl:for-each select="//ProjectInformation/LanguageFiles/LanguageFile">
							<tr>
								<td>
									<xsl:value-of select="@Name"/>
								</td>
								<td>
									<xsl:value-of select="@TargetLanguage"/>
								</td>
								<td>
									<xsl:value-of select="@RunAt"/>
								</td>
								<td>
									<xsl:value-of select="Phase/@AssignedPhase"/>
								</td>
								<td>
									<xsl:value-of select="Phase/@IsCurrentAssignment"/>
								</td>
								<td>
									<xsl:value-of select="Phase/@AssigneesNumber"/>
								</td>
								<td>
									<xsl:value-of select="NumberVerifier/@ExecutedDate"/>
								</td>
							</tr>
						</xsl:for-each>
					</table>
				</div>
			
			</body>
    </html>
  </xsl:template>
</xsl:stylesheet>
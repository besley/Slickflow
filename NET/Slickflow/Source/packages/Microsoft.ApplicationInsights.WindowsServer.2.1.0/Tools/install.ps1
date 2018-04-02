param($installPath, $toolsPath, $package, $project)

if ([System.IO.File]::Exists($project.FullName))
{
	function MarkItemASCopyToOutput($item)
	{
		Try
		{
			#mark it to copy if newer
			$item.Properties.Item("CopyToOutputDirectory").Value = 2
		}
		Catch
		{
			write-host $_.Exception.ToString()			
		}
	}

	MarkItemASCopyToOutput($project.ProjectItems.Item("ApplicationInsights.config"))
}
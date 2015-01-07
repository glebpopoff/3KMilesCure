<?php
	/**
	* MOCK View Processor: .NET Razor Template
	*/
	function get_angular_app($url)
	{
		$angular_apps = array(
							  'home'=>array('name'=>'homepageApp', 'script'=>'/ui/app/homepage/js/main'),
							  );
		
		switch ($url)
		{
			default:
				return $angular_apps['home'];
				
		}
	}

	$app = get_angular_app($_SERVER['REQUEST_URI']);
	$html  = file_get_contents($_SERVER['DOCUMENT_ROOT'] . '/DonationPortal.Web/Views/Page/Index.cshtml');
	$html = str_replace('@Model.AngularAppName', $app['name'], $html);
	$html = str_replace('@Model.AngularAppMainScript', $app['script'], $html);
	$html = str_replace('@model 3KMilesCure.Website.Models.MainAppViewModel', '', $html);
	echo $html;
	exit;
?>
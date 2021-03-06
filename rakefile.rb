require 'albacore'
require 'nokogiri'

$connection_string = ENV['CONNECTION_STRING']
$aws_access_key = ENV['AWS_ACCESS_KEY']
$aws_secret_key = ENV['AWS_SECRET_KEY']

task :default => [:restore, :compile_this, :transform_config, :deploy]

#https://github.com/Albacore/albacore/wiki/nugets_restore
nugets_restore :restore do |p|
  p.out       = 'packages'
  p.nuget_gem_exe
end

#https://github.com/Albacore/albacore/wiki/build
build :compile_this do |b|
  b.file   = Paths.join 'StockSharer.Web', 'StockSharer.Web.csproj'
  b.target = ['Clean', 'Rebuild']
  b.prop 'Configuration', 'Release'
  b.nologo
  b.be_quiet
  b.prop 'UseWPP_CopyWebApplication', 'true'
  b.prop 'PipelineDependsOnBuild', 'false'
  b.prop 'webprojectoutputdir', '../deploy/'
  b.prop 'outdir', 'bin/'
end

#http://www.nokogiri.org/tutorials/modifying_an_html_xml_document.html
task :transform_config do
  filename = "deploy/Web.config"
  doc = File.open(filename) { |f| Nokogiri::XML(f) }
  connectionStringNode = doc.xpath("//connectionStrings/add[@name='StockSharerDatabase']")[0]
  connectionStringNode['connectionString'] = $connection_string
  
  awsAccessKeyNode = doc.xpath("//appSettings/add[@key='AWSAccessKey']")[0]
  awsAccessKeyNode['value'] = $aws_access_key
  
  awsSecretKeyNode = doc.xpath("//appSettings/add[@key='AWSSecretKey']")[0]
  awsSecretKeyNode['value'] = $aws_secret_key
  
  File.write(filename, doc.to_xml)
end

task :deploy do
  FileUtils.cp_r 'deploy/.', 'C:/inetpub/wwwroot'
end
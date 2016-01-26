require 'albacore'
require 'nokogiri'

$connection_string = ENV['CONNECTION_STRING']
$smtp_username = ENV['SMTP_USERNAME']
$smtp_password = ENV['SMTP_PASSWORD']

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
  
  networkNode = doc.xpath("//smtp/network")[0]
  networkNode['userName'] = $smtp_username
  networkNode['password'] = $smtp_password
  
  File.write(filename, doc.to_xml)
end

task :deploy do
  FileUtils.cp_r 'deploy/.', 'C:/inetpub/wwwroot'
end
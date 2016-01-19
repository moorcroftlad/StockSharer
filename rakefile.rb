require 'albacore'
require 'nokogiri'

$connection_string = ENV['CONNECTION_STRING']

task :default => [:transform_config, :restore, :compile_this]

#http://www.nokogiri.org/tutorials/modifying_an_html_xml_document.html
task :transform_config do
  filename = "deploy/Web.config"
  doc = File.open(filename) { |f| Nokogiri::XML(f) }
  node = doc.xpath("//connectionStrings/add[@name='StockSharerDatabase']")[0]
  node['connectionString'] = $connection_string
  File.write(filename, doc.to_xml)
  puts $connection_string
end

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
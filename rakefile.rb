require 'albacore'

task :default => [:restore, :compile_this]

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
end
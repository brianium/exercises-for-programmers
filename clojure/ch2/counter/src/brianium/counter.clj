(ns brianium.counter
  (:require [brianium.counter.cli :as cli])
  (:gen-class))

(defn -main
  [& args]
  (cli/run))

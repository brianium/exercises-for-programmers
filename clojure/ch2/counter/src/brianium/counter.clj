(ns brianium.counter
  (:require [brianium.counter.cli :as cli]
            [brianium.counter.gui :as gui])
  (:gen-class))

(defn -main
  [& args]
  (let [view (first args)]
    (if (= view "gui")
      (gui/run)
      (cli/run))))

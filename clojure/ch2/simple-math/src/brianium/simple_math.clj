(ns brianium.simple-math
  (:require [brianium.simple-math.cli :as cli]
            [brianium.simple-math.gui :as gui])
  (:gen-class))

(defn -main
  "I don't do a whole lot ... yet."
  [& args]
  (let [view (first args)]
    (if (= view "gui")
      (gui/run)
      (cli/run))))
